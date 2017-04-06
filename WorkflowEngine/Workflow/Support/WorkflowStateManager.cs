using System.Collections.Generic;
using WorkflowEngine.Workflow.Engine.WorkflowStates;
using WorkflowEngine.Workflow.Model.WorkflowStates;

namespace WorkflowEngine.Workflow.Support
{
    public class WorkflowStateManager
    {
        public WorkflowState CurrentWorkflowState { get; private set; }

        public WorkflowStateManager(WorkflowState currentWorkflowState )
        {
            CurrentWorkflowState = currentWorkflowState;
        }

        public List<WorkflowStateConfig> WorkflowStatesHistory { get; set; }
        public List<WorkflowTransitionConfig> WorkflowTransitionHistory { get; set; }

        public WorkflowStateManager MirgrateState(WorkflowState destinationState)
        {
            return new WorkflowStateManager(destinationState);
        }

        public static WorkflowState BuildState(WorkflowStateConfig config)
        {
            return new WorkflowState(() => config);
        }
    }
}