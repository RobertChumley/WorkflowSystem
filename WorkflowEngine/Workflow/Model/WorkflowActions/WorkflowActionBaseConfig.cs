using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkflowEngine.Workflow.Model.Parameters;
using WorkflowEngine.Workflow.Model.Types;

namespace WorkflowEngine.Workflow.Model.WorkflowActions
{
    public abstract class WorkflowActionBaseConfig
    {
        [Key]
        public int Id { get; set; }
        public string WorkflowActionName { get; set; }
        public List<WorkflowParameter> InboundParameters { get; set; }
        public List<WorkflowParameter> ReturnValues { get; set; }
        public string NextAction { get; set; }
        public WorkflowActionType WorkflowActionType { get; set; }
        public WorkflowRoutine WorkflowRoutine { get; set; }
    }
}
