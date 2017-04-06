using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkflowEngine.Workflow.Model.WorkflowActions
{
    public class WorkflowTransitionActionConfig : WorkflowActionBaseConfig
    {
        public string DestinationState { get; set; }
    }

}
