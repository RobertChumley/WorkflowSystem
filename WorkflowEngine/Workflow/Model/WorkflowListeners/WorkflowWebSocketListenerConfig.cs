namespace WorkflowEngine.Workflow.Model.WorkflowListeners
{
    public class WorkflowWebSocketListenerConfig : WorkflowListenerConfigBase
    {
        public string IPAddress { get; set; }
        public int PortNumber { get; set; }
        public string Protocol { get; set; }
    }
}