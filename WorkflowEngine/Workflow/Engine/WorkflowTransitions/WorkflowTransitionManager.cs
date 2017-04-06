using WorkflowEngine.Workflow.Model.WorkflowStates;

namespace WorkflowEngine.Workflow.Engine.WorkflowTransitions
{
    public class WorkflowTransitionManager
    {
        public static WorkflowTransition BuildTransition(WorkflowTransitionConfig config)
        {
            return new WorkflowTransition(()=> config);
        }
    }
}