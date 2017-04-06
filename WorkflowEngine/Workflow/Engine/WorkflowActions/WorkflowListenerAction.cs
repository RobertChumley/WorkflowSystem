using System;
using WorkflowEngine.Workflow.Engine.WorkflowRegistries;
using WorkflowEngine.Workflow.Model.Types;
using WorkflowEngine.Workflow.Model.WorkflowActions;

namespace WorkflowEngine.Workflow.Engine.WorkflowActions
{
    public class WorkflowListenerAction : WorkflowAction
    {
        public WorkflowListenerAction(Func<WorkflowActionBaseConfig> workflowActionConfiguration, WorkflowEventType workflowEventType) :
            base( workflowActionConfiguration)
        {
            WorkflowEventType = workflowEventType;
            
        }

        public WorkflowEventType WorkflowEventType { get; }
        
        //ToDo:  Figure out exactly what this is supposed to do.  There should be an event action which raises events, but the EventActionExecution should be listening for events
        
        public Func<WorkflowAction> Execute()
        {
            return () => WorkflowActionRegistry()[WorkflowActionConfiguration().NextAction];
        }
    }
}