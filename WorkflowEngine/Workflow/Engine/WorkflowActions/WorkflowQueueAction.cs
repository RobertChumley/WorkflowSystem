using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KafkaNet;
using KafkaNet.Model;
using KafkaNet.Protocol;
using WorkflowEngine.Workflow.Model.WorkflowActions;

namespace WorkflowEngine.Workflow.Engine.WorkflowActions
{
    public class WorkflowQueueAction : WorkflowAction
    {
        public WorkflowQueueAction(Func<WorkflowActionBaseConfig> workflowActionConfiguration) : base(workflowActionConfiguration)
        {
            WorkflowQueueActionConfig = (WorkflowQueueActionConfig)workflowActionConfiguration();
        }

        public WorkflowQueueActionConfig WorkflowQueueActionConfig { get; set; }
        public Func<WorkflowAction> Execute()
        {
            
            var options = new KafkaOptions(WorkflowQueueActionConfig.Servers.Select(i => new Uri(i.Url)).ToArray());
            var router = new BrokerRouter(options);
            var client = new Producer(router);

            client.SendMessageAsync(WorkflowQueueActionConfig.QueueName, new[] { new Message(WorkflowQueueActionConfig.QueueAction) }).Wait();

            using (client)
            {
               
            }
            return () => WorkflowActionRegistry()[WorkflowActionConfiguration().NextAction];
        }

    }

    public enum QueueType
    {
        Kafka,MSMQ,RabbitMQ
    }

    public class ServerConfig
    {
        [Key]
        public int Id { get; set; }
        public string Url { get; set; }
    }
}
