using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using WorkflowEngine.Workflow.Model;
using WorkflowEngine.Workflow.Model.WorkflowObjects;

namespace WorkflowEngine.DatabaseEngine
{
    public class DatabaseTableBuilder
    {
        public DatabaseTableBuilder(DatabaseConfig databaseConfig)
        {
            DatabaseConfig = databaseConfig;
        }
        public DatabaseConfig DatabaseConfig { get; }

        public Func<SqlConnection> GetConnection()
        {
            return () =>
            {
                var connection = new SqlConnection(DatabaseConfig.ConnectionString);
                connection.Open();
                return connection;
            };
        }

        public void BuildSQLTables(Dictionary<string, WorkflowObjectType> fields)
        {
            GetConnection()().ExecuteCommands(BuildSQLTableExpression()(fields));
            
        }
        public Func<Dictionary<string, WorkflowObjectType>,List<string>> BuildSQLTableExpression()
        {
                return (fields) =>
                {
                    return fields.Keys.ToList().Select(key =>
                    {
                        return $"Create Table {fields[key].ObjectTypeName} ( {fields[key].PrimaryKeyField} int IDENTITY(1,1), { string.Join(",", fields[key].ObjectFields.Select(i => i.FieldName + " " + i.MappedDBType() + " null") ) } )";   
                    }).ToList();
                };
         }
    }

    public static class CommandExtensions
    {
        public static void ExecuteCommands(this SqlConnection connection, List<string> commands)
        {
            using (var command = connection.CreateCommand())
            {
                commands.ForEach(commandStr =>
                {
                    command.CommandText = commandStr;
                    command.CommandType = CommandType.Text;
                    command.ExecuteNonQuery();
                });
            }
        }
    }
    public class DatabaseConfig
    {
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
    }
}
