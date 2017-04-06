using System;
using WorkflowEngine.Workflow.Engine.WorkflowContainer;
using WorkflowEngine.Workflow.Engine.WorkflowRegistries;
using WorkflowEngine.Workflow.Model.Types;
using WorkflowEngine.Workflow.Model.WorkflowActions;
using WorkflowEngine.Workflow.Support;

namespace WorkflowEngine.Workflow.Engine.WorkflowActions
{
    public class WorkflowTransitionAction : WorkflowAction
    {
        private readonly Action<WorkflowStateManager> _setNewManager;

        public WorkflowTransitionAction(
            Func<WorkflowActionBaseConfig> workflowActionConfiguration,Action<WorkflowStateManager> setNewManager) : base(workflowActionConfiguration)
        {
            _setNewManager = setNewManager;
            WorkflowStateRegistry = RegistryContainer.Resolve<WorkflowStateRegistry>;
            WorkflowTransitionActionConfig = (WorkflowTransitionActionConfig)workflowActionConfiguration();
        }
        public Func<WorkflowStateRegistry> WorkflowStateRegistry { get; }
        public WorkflowTransitionActionConfig WorkflowTransitionActionConfig { get; }
        public Func<WorkflowAction> Execute(Func<WorkflowStateManager> stateManager,Action<WorkflowEventType> eventAction)
        {
            eventAction(WorkflowEventType.TransitionOut);
            _setNewManager(stateManager().MirgrateState(WorkflowStateRegistry()[WorkflowTransitionActionConfig.DestinationState]));
            eventAction(WorkflowEventType.TransitionIn);
            return () => WorkflowActionRegistry()[WorkflowActionConfiguration().NextAction];
        }
        
    }
}