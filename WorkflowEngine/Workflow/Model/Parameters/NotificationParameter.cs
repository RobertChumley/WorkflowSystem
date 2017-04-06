using System.Security;

namespace WorkflowEngine.Workflow.Model.Parameters
{
    public class NotificationParameter : GenericParamter
    {
        public NotificationParameter(string name, string type, object value)
        {
            Name = name;
            Type = type;
            Value = value;
        }
    }
}