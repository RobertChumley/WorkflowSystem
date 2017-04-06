using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkflowEngine.Workflow.Model.Types;

namespace WorkflowEngine.Workflow.Model.WorkflowActions
{
    public class WorkflowEventActionConfig : WorkflowActionBaseConfig
    {
        public string EventName { get; set; }
        public WorkflowEventType WorkflowEventType { get; set; }
        
    }
}
