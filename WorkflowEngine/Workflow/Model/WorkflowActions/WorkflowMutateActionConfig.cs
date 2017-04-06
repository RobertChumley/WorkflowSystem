using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkflowEngine.Workflow.Model.Parameters;
using WorkflowEngine.Workflow.Model.Types;

namespace WorkflowEngine.Workflow.Model.WorkflowActions
{
    public class WorkflowMutateActionConfig : WorkflowActionBaseConfig
    {
        public MutationOperatorType MutationOperatorType { get; set; }
        public WorkflowParameter MutationParameter { get; set; }

    }
}
