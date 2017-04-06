using System;
using WorkflowEngine.Workflow.ExecutionSystem;
using WorkflowEngine.Workflow.Model.WorkflowActions;
using WorkflowEngine.Workflow.Support;

namespace WorkflowEngine.Workflow.Engine.WorkflowActions
{
    public class WorkflowNotificationAction : WorkflowAction
    {
        public WorkflowNotificationAction(Func<WorkflowActionBaseConfig> workflowActionConfiguration) :
            base(workflowActionConfiguration)
        {
        }
        public WorkflowNotifier WorkflowNotifier { get; set; }
        public WorkflowNotification WorkflowNotification { get; set; }
        
        public Func<WorkflowAction> Execute()
        {
            WorkflowNotifier.Notify(WorkflowNotification);
            return () => WorkflowActionRegistry()[WorkflowActionConfiguration().NextAction];
        }
    }
}