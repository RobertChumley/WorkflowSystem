namespace WorkflowEngine.Workflow.Model.WorkflowStates
{
    public class WorkflowTransitionConfig
    {
        public string TranssitionReason { get; set; }
        public string SourceState { get; set; }
        public string DestinationState { get; set; }
        public string OnTransitionInEventName { get; set; }
        public string OnTransitionOutEventName { get; set; }
    }
}