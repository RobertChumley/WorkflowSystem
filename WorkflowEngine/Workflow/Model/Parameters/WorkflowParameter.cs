using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WorkflowEngine.Workflow.Model.Parameters
{
    public class WorkflowParameter
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }

        public string Type { get; set; }
        [NotMapped]
        public object Value { get; set; }
    }
}