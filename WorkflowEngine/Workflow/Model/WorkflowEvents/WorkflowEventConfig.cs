using System.Collections.Generic;
using WorkflowEngine.Workflow.Engine.WorkflowActions;
using WorkflowEngine.Workflow.Model.Types;

namespace WorkflowEngine.Workflow.Model.WorkflowEvents
{
    public class WorkflowEventConfig
    {
        public string EventName { get; set; }
        public WorkflowEventType WorkflowEventType { get; set; }
        public string WorkflowActionName { get; set; }
    }
}