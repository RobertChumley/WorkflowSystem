using System;
using WorkflowEngine.Workflow.Model.Parameters;
using WorkflowEngine.Workflow.Model.Types;
using WorkflowEngine.Workflow.Model.WorkflowActions;
using WorkflowEngine.Workflow.Support;

namespace WorkflowEngine.Workflow.Engine.WorkflowActions
{
    public class WorkflowMutateStateAction : WorkflowAction
    {
        public WorkflowMutateStateAction(Func<WorkflowActionBaseConfig> workflowActionConfiguration) :
            base(workflowActionConfiguration)
        {
            var actionConfig = (WorkflowMutateActionConfig) workflowActionConfiguration();
            MutationOperatorType = actionConfig.MutationOperatorType;
            MutationParameter = actionConfig.MutationParameter;
        }

        public MutationOperatorType MutationOperatorType { get; }
        public WorkflowParameter MutationParameter  { get; }

        public Func<WorkflowAction> Execute(Func<WorkflowStateManager> stateManager)
        {
            stateManager().CurrentWorkflowState.Mutate(MutationOperatorType)(MutationParameter);
            return () => WorkflowActionRegistry()[WorkflowActionConfiguration().NextAction];
        }
    }
}