using System.Collections;
using System.Collections.Generic;
using System.Linq;
using WorkflowEngine.Workflow.Model.WorkflowObjects;

namespace WorkflowEngine.Workflow.Listeners
{
    public class WorkflowDataObject : DictionaryBase
    {
        public WorkflowDataObject(
            WorkflowObjectType workflowObjectType, 
            Dictionary<string, object> fieldValues,
            string primaryKeyValue)
        {
            WorkflowObjectType = workflowObjectType;
            if(fieldValues != null)
                FieldValues = workflowObjectType.ObjectFields.ToDictionary(i => i.FieldName, i =>
                    new WorkflowObjectValue(i.FieldName, i.FieldType,
                        fieldValues.ContainsKey(i.FieldName) ? fieldValues[i.FieldName] : null));
            else
                FieldValues = new Dictionary<string, WorkflowObjectValue>();

            PrimaryKeyValue = primaryKeyValue;
        }

        public WorkflowObjectType WorkflowObjectType { get; }

        public Dictionary<string, WorkflowObjectValue> FieldValues { get; }

        public string PrimaryKeyField => WorkflowObjectType.PrimaryKeyField;
        public string PrimaryKeyValue { get;  }

        public WorkflowObjectValue this[string key]
            => key != null && FieldValues.ContainsKey(key) ? FieldValues[key] : null;

        public Dictionary<string, WorkflowObjectValue>.KeyCollection Keys()
        {
            return FieldValues.Keys;
        }
    }
}