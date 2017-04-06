using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using WorkflowEngine.Workflow.Model.Parameters;
using WorkflowEngine.Workflow.Model.Rules;
using WorkflowEngine.Workflow.Model.Types;

namespace WorkflowEngine.Workflow.Model.WorkflowStates
{
    public class WorkflowStateConfig
    {
        public WorkflowStateConfig()
        {
            WorkflowParameters =new List<WorkflowParameter>();
            WorkflowExecutionRules = new List<WorkflowRuleConfig>();
        }
        [Key]
        public int Id { get; set; }
        public string StateName { get; set; }
        public List<WorkflowParameter> WorkflowParameters { get; set; }
        public WorkflowStateType WorkflowStateType { get; set; }
        public List<WorkflowRuleConfig> WorkflowExecutionRules { get; set; }
    }
}
