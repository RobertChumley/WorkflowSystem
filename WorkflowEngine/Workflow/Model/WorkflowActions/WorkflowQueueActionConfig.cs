using System.Collections.Generic;
using WorkflowEngine.Workflow.Engine.WorkflowActions;

namespace WorkflowEngine.Workflow.Model.WorkflowActions
{
    public class WorkflowQueueActionConfig : WorkflowActionBaseConfig
    {
        public List<ServerConfig> Servers { get; set; }
        public QueueType QueueType { get; set; }
        public string QueueName { get; set; }
        public string QueueAction { get; set; }
    }
}