using System.Collections.Generic;
using WorkflowEngine.Workflow.Model.Parameters;
using WorkflowEngine.Workflow.Model.Types;

namespace WorkflowEngine.Workflow.Support
{
    public class WorkflowNotification
    {
        public WorkflowNotificationType WorkflowNotificationType { get; set; }
        public IList<NotificationParameter> WorkfloNotificationParameters { get; set; }
    }
}