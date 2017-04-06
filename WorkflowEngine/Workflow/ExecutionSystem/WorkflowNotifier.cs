using System;
using WorkflowEngine.Workflow.Model.Types;
using WorkflowEngine.Workflow.Support;

namespace WorkflowEngine.Workflow.ExecutionSystem
{
    public class WorkflowNotifier
    {
        public void Notify(WorkflowNotification workflowNotification)
        {
            switch (workflowNotification.WorkflowNotificationType)
            {
                case WorkflowNotificationType.Email:
                    break;
                case WorkflowNotificationType.HttpPost:
                    break;
                case WorkflowNotificationType.HttpGet:
                    break;
                case WorkflowNotificationType.Database:
                    break;
                case WorkflowNotificationType.Log:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
