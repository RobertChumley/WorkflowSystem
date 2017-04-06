using System;
using WorkflowEngine.Workflow.Engine.WorkflowContainer;
using WorkflowEngine.Workflow.Engine.WorkflowRegistries;
using WorkflowEngine.Workflow.Model.Types;
using WorkflowEngine.Workflow.Model.WorkflowActions;
using WorkflowEngine.Workflow.Model.WorkflowEvents;

namespace WorkflowEngine.Workflow.Engine.WorkflowActions
{
    public class WorkflowEventAction : WorkflowAction
    {
        public WorkflowEventActionConfig WorkflowEventActionConfig { get; set; }

        public WorkflowEventAction(Func<WorkflowActionBaseConfig> workflowActionConfiguration) : base(workflowActionConfiguration)
        {
            WorkflowEventActionConfig = (WorkflowEventActionConfig)workflowActionConfiguration();
        }
        //ToDO: Hook this into the eventing system
        public Func<WorkflowAction> Execute(Action<WorkflowEventType> actionFunc)
        {
            actionFunc(WorkflowEventActionConfig.WorkflowEventType);
            return () => WorkflowActionRegistry()[WorkflowActionConfiguration().NextAction];
        }
    }
}