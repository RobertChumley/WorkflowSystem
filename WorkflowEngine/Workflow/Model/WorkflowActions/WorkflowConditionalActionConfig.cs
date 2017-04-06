using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkflowEngine.Workflow.Model.WorkflowEvents;

namespace WorkflowEngine.Workflow.Model.WorkflowActions
{
    public class WorkflowConditionalActionConfig : WorkflowActionBaseConfig
    {
        public WorkflowConditionConfig WorkflowConditions { get; set; }
    }
}
