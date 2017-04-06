using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkflowEngine.Workflow.Model.WorkflowActions;

namespace WorkflowEngine.Workflow.Model.WorkflowListeners
{
    public abstract class WorkflowListenerConfigBase
    {
        [Key]
        public int Id { get; set; }
        public ListenerType ListenerType { get; set; }
        public string WorkflowObjectName { get; set; }
        public List<ListenerActionMap> ListenerActions { get; set; }
        public string WorkflowListenerName { get; set; }
    }

    public enum ListenerType
    {
        WebAPI, Queue, WebSocket,
        SignalR,
        FileSystem,
        AWSS3,
        AzureBlob,
        Socket
    }

    public class ListenerActionMap
    {
        [Key]
        public int Id { get; set; }
        public string ListenerMethod { get; set; }
        public string ListenerAction { get; set; }
    }

}
