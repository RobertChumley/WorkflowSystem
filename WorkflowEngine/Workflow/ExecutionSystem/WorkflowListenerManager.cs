using System;
using WorkflowEngine.Workflow.Listeners;
using WorkflowEngine.Workflow.Model.WorkflowListeners;

namespace WorkflowEngine.Workflow.ExecutionSystem
{
    public class WorkflowListenerManager
    {
        public static WorkflowListenerBase BuildListener(WorkflowListenerConfigBase config)
        {
            switch (config.ListenerType)
            {
                case ListenerType.WebAPI:
                    return new WebApiListener(()=>config);
                case ListenerType.Queue:
                    break;
                case ListenerType.WebSocket:
                    return new WorkflowWebSocketListener(() => config);
                case ListenerType.SignalR:
                    break;
                case ListenerType.FileSystem:
                    break;
                case ListenerType.AWSS3:
                    break;
                case ListenerType.AzureBlob:
                    break;
                case ListenerType.Socket:
                    return new WorkflowSocketListener(() =>config);
                default:
                    throw new ArgumentOutOfRangeException();
            }
            return null;
        }
    }
}
