using System.ComponentModel.DataAnnotations;

namespace WorkflowEngine.Workflow.Model.WorkflowActions
{
    public class WorkflowRoutine
    {
        [Key]
        public int Id { get; set; }
        public string MethodName { get; set; }
        public string TypeName { get; set; }
    }
}