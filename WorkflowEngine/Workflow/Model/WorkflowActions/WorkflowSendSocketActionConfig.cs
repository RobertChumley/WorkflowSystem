using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkflowEngine.Workflow.Model.WorkflowActions
{
    public class WorkflowSendSocketActionConfig : WorkflowActionBaseConfig
    {
        public string IPAddress { get; set; }
        public int PortNumber { get; set; }
        public string Protocol { get; set; }

    }
}
