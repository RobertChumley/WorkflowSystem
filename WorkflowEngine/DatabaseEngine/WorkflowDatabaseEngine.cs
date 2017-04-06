using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using WorkflowEngine.Workflow.Listeners;


namespace WorkflowEngine.DatabaseEngine
{
    public class WorkflowDatabaseEngine
    {
        public WorkflowDatabaseEngine(Func<WorkflowSQLConnection> workflowSqlConnection)
        {
            WorkflowSqlConnection = workflowSqlConnection;
        }

        public Func<WorkflowSQLConnection> WorkflowSqlConnection { get; set; }


        public WorkflowDatabaseEngine InsertData(WorkflowDataObject workflowDataObject)
        {
            WorkflowSqlConnection().ExecuteInsertCommand(workflowDataObject.WorkflowObjectType.ObjectTypeName,
                workflowDataObject.FieldValues.ToFieldDictionary());
            return this;
        }

        public WorkflowDatabaseEngine UpdateData(WorkflowDataObject workflowDataObject)
        {
            WorkflowSqlConnection().ExecuteUpdateCommand(
                workflowDataObject.WorkflowObjectType.ObjectTypeName,
                workflowDataObject.FieldValues.ToFieldDictionary(), 
                workflowDataObject.PrimaryKeyField,
                workflowDataObject.PrimaryKeyValue);
            return this;
        }

        public void DeleteData(WorkflowDataObject workflowDataObject)
        {
            WorkflowSqlConnection()
                .ExeuteDeleteCommand(workflowDataObject.WorkflowObjectType.ObjectTypeName, workflowDataObject.PrimaryKeyField,
                    workflowDataObject.PrimaryKeyValue);
        }

        public DataTable ReadData(WorkflowDataObject workflowDataObject)
        {
            return WorkflowSqlConnection().ExecuteDataTable(
                workflowDataObject.WorkflowObjectType.ObjectTypeName,
                workflowDataObject.FieldValues.ToFieldDictionary(),
                workflowDataObject.PrimaryKeyField,
                workflowDataObject.PrimaryKeyValue);

        }
    }

    public static class FieldDictionaryExtensions
    {
        public static Dictionary<string, string> ToFieldDictionary(this Dictionary<string, WorkflowObjectValue> data)
        {
            return data.ToDictionary(i => i.Value.FieldName, i => i.Value.ObjectValue?.ToString() ?? "");
        }
    }
        
}
