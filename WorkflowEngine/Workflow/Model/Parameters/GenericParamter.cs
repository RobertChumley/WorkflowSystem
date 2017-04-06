using System.Collections.Specialized;

namespace WorkflowEngine.Workflow.Model.Parameters
{
    public abstract class GenericParamter
    {
        

        public string Name { get; set; }
        public string Type { get; set; }
        public object Value { get; set; }
    }
}