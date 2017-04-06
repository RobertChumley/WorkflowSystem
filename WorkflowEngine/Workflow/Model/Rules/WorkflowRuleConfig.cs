using System.ComponentModel.DataAnnotations;
using WorkflowEngine.Workflow.Model.Parameters;
using WorkflowEngine.Workflow.Model.Types;

namespace WorkflowEngine.Workflow.Model.Rules
{
    public class WorkflowRuleConfig
    {
        [Key]
        public int Id { get; set; }
        public WorkflowRuleOperator WorkflowRuleOperator { get; set; }
        public WorkflowParameter LeftHand { get; set; }
        public WorkflowParameter RightHand { get; set; }
        public string PositiveAction { get; set; }
        public string ElseAction { get; set; }
        public string LeftHandParam { get; set; }
    }
}
