using WorkflowEngine.Workflow.Model.WorkflowObjects;

namespace WorkflowEngine.Workflow.Engine.WorkflowObjects
{
    public abstract class WorkflowObject
    {
        public WorkflowObjectType WorkflowObjectType { get; set; }
        public WorkflowRawObjectType WorkflowRawObjectType  { get; set; }
    }
}