using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;

namespace WorkflowEngine.DatabaseEngine
{
    public enum ConnectionType
    {
        Application, System
    }
    public class WorkflowSQLConnection : IDisposable
    {
        //connection =
        //       new SqlConnection(
        //           "Data Source=tcp:gaapdev.database.windows.net,1433;initial catalog=GAAPDEV;persist security info=True;user id=gaap_admin@gaapdev;password=zmd9gHbUUh7b;MultipleActiveResultSets=True;App=EntityFramework");
        private readonly string _prefix;
        private readonly SqlConnection _connection;


        public WorkflowSQLConnection(DatabaseConfig databaseConfig) 
        {
            _connection = new SqlConnection(databaseConfig.ConnectionString);
            _connection.Open();
        }

        
        protected Dictionary<string, string> GetValues<T>(T obj)
        {
            var result = new Dictionary<string, string>();
            var props = obj.GetType().GetProperties();
            props.ToList().ForEach(prop => result[prop.Name] = FormatOutput(prop, obj));
            return result;
        }
        protected Dictionary<string, string> GetValues<T>(T obj,string exclude)
        {
            var result = new Dictionary<string, string>();
            var props = obj.GetType().GetProperties();
            props.ToList().Where(i=>i.Name != exclude).ToList().ForEach(prop => result[prop.Name] = FormatOutput(prop, obj));
            return result;
        }
        private string FormatOutput(PropertyInfo prop, object obj)
        {
            var value = prop.GetValue(obj, null);
            if (prop.PropertyType.Name == typeof(DateTime).Name)
            {
                return ((DateTime) value).Date == DateTime.MinValue
                    ? null
                    : ((DateTime) value).Date.ToString("yyyy-MM-dd HH:mm:ss");
            }
            if (prop.PropertyType.FullName == typeof(Nullable<DateTime>).FullName)
            {
                var nulValue = (DateTime?) value;
                return nulValue?.Date.ToString("yyyy-MM-dd HH:mm:ss");
            }
            else
            {
                return (value ?? "").ToString();
            }
        }

        public int ExeuteDeleteCommand(string table, string fieldName, string id)
        {
            using (var command = _connection.CreateCommand())
            {
                command.CommandType = CommandType.Text;
                command.CommandText = DeleteStr(table, fieldName, id);
                return command.ExecuteNonQuery();
            }
        }

        public T ExecuteSingleRow<T>(int id, string table, string field) where T : class, new()
        {
            return ExecuteSingleRow<T>(id.ToString(), table, field);
        }
        public T ExecuteSingleRow<T>(string id, string table, string field) where T : class, new()
        {
            using (var command = _connection.CreateCommand())
            {
                command.CommandType = CommandType.Text;
                command.CommandText = GetByValue(id, table, field);
                return command.ExecuteReader().ToBoundList<T>().FirstOrDefault();
            }
        }
        public T ExecuteSQLSelect<T>(string query) where T : class, new()
        {
            using (var command = _connection.CreateCommand())
            {
                command.CommandType = CommandType.Text;
                command.CommandText = query;
                return command.ExecuteReader().ToBoundList<T>().FirstOrDefault();
            }
        }

        public void ExecuteInsertCommand(string table, Dictionary<string, string> nvpairs)
        {
            using (var command = _connection.CreateCommand())
            {
                command.CommandType = CommandType.Text;
                command.CommandText = InsertStr(table, nvpairs);
                command.ExecuteNonQuery();
            }
        }

        public void ExecuteUpdateCommand(string table, Dictionary<string, string> nvpairs, string primaryKeyField, string primaryKeyValue)
        {
            using (var command = _connection.CreateCommand())
            {
                command.CommandType = CommandType.Text;
                command.CommandText = UpdateStr(table, nvpairs, primaryKeyField,primaryKeyValue);
                command.ExecuteNonQuery();
            }
        }

        public List<T> ExecuteBasicSelectCommand<T>(string table, string[] selectFields, Dictionary<string, string> nvpairs) where T : class, new()
        {
            using (var command = _connection.CreateCommand())
            {
                command.CommandType = CommandType.Text;
                command.CommandText = SelectStr(table, selectFields, nvpairs);
                return command.ExecuteReader().ToBoundList<T>().ToList();
            }
        }

        protected DataTable ExecuteDataTable(string tableName, string columnName, string searchValue)
        {
            var table = new DataTable();
            
            using (var da = new SqlDataAdapter($"SELECT * FROM {tableName} WHERE {columnName} like '%{searchValue}%' order by {columnName} asc", _connection.ConnectionString)) {
                da.Fill(table);
            }
            return table;
        }
        public DataTable ExecuteDataTable(string tableName, Dictionary<string, string> nvpairs, string columnName, string searchValue)
        {
            var table = new DataTable();
            var fields = new List<string>();
            nvpairs.Keys.ToList().ForEach(i =>
            {
                if(nvpairs[i] != null)
                    fields.Add($"{i} like '%{nvpairs[i]}%' ");
            });
            using (var da = new SqlDataAdapter($"SELECT {columnName}, {string.Join(",", nvpairs.Keys.ToList())} " +
                                               $"FROM {tableName} " +
                                               $"WHERE {string.Join(" and ",fields)} ", _connection.ConnectionString))
            {
                da.Fill(table);
            }
            return table;
        }
        protected DataTable ExecuteDataTable(string tableName, List<string> selColumnsList, string columnName, string searchValue)
        {
            var table = new DataTable();
            var selStr = string.Join(",",selColumnsList);
            using (var da = new SqlDataAdapter($"SELECT top 26 {selStr} FROM {tableName} WHERE {columnName} like '%{searchValue}%' order by {columnName} asc", _connection.ConnectionString))
            {
                da.Fill(table);
            }
            return table;
        }

        private string DeleteStr(string table, string fieldName, string id)
        {
            return $"delete from {_prefix}.{table} where {fieldName} = '{id}'";
        }

        private string UpdateStr(string table, Dictionary<string, string> nvpairs, string primaryKeyField, string primaryKeyValue)
        {
            var pairs =
                nvpairs.Keys.ToList()
                    .Where(key => key.ToLower() != primaryKeyField.ToLower())
                    .Select(key => $" {key}='{nvpairs[key]}' ");
            var updateValues = string.Join(",", pairs);
            return $"update {_prefix}.{table} set {updateValues} where {primaryKeyField} = '{primaryKeyValue}'";
        }

        private string InsertStr(string table, Dictionary<string, string> nvpairs)
        {
            var fields = string.Join(",", nvpairs.Keys);
            var values = nvpairs.Values.ToList().Select(value => $"'{value}'");
            var updateValues = string.Join(",", values);
            return $"insert into  {_prefix}.{table} ({fields}) values ({updateValues})";
        }

        private string SelectStr(string table, string[] selectFields, Dictionary<string, string> nvpairs)
        {
            var pairs = nvpairs.Keys.ToList().Select(key => $"{key}='{nvpairs[key]}'");
            var updateValues = string.Join(" and ", pairs);
            var fields = string.Join(",", selectFields);
            return $"select {fields} from {_prefix}.{table} where {updateValues} ";
        }

        private string GetByValue(string value, string table, string singleField)
        {
            return $"select * from {_prefix}.{table} where {singleField} = '{value}' ";
        }

        public void Dispose()
        {
            _connection.Close();
        }
    }

    public static class IEnumerableExtensions
    {
        public static IEnumerable<T> ToBoundList<T>(this DbDataReader dr)
        {
            Reflection reflec = new Reflection();
            Object instance;
            List<T> lstObj = new List<T>();
            while (dr.Read())
            {
                instance = (T) Activator.CreateInstance(typeof(T));
                foreach (DataRow drow in dr.GetSchemaTable().Rows)
                {
                    reflec.FillObjectWithProperty(ref instance, drow.ItemArray[0].ToString(),
                        dr[drow.ItemArray[0].ToString()]);
                }
                lstObj.Add((T) Convert.ChangeType(instance, typeof(T)));
            }
            return lstObj;
        }
    }

    public class Reflection
    {
        public void FillObjectWithProperty(ref object objectTo, string propertyName, object propertyValue)
        {
            if (propertyValue is DBNull)
                return;
            if (propertyValue == null)
                return;
            var prop = objectTo.GetType()
                .GetProperties()
                .FirstOrDefault(i => i.Name.ToLower() == propertyName.ToLower());
            var props = TypeDescriptor.GetProperties(objectTo);
            var prop2 = props.Find(propertyName, true);
            if (prop2.Converter != null)
                prop?.SetValue(objectTo, prop2.Converter.ConvertFromInvariantString(propertyValue.ToString()), null);
        }
    }
}
