namespace WorkflowEngine.Workflow.Listeners
{
    public class WorkflowObjectValue
    {
        public WorkflowObjectValue(string fieldName,string objectType, object objectValue)
        {
            FieldName = fieldName;
            ObjectType = objectType;
            ObjectValue = objectValue;
        }

        public string FieldName { get;  }
        public string ObjectType { get; }
        public object ObjectValue { get;}
    }
}