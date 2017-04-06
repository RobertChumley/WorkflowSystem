namespace WorkflowEngine.Workflow.Model.Types
{
    public enum WorkflowActionType
    {
        Execute,
        Conditional,
        Transition,
        Notification,
        Listener,
        MutateState,
        Rule,
        Timer,
        Event,
        Queue,
        SendSocket
    }
}