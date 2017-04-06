using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WorkflowEngine.Workflow.Model.WorkflowObjects
{
    
    public class WorkflowObjectType
    {
        [Key]
        public int Id { get; set; }
        public string ObjectTypeName { get; set; }
        public List<ObjectField> ObjectFields { get; set; }
        public string PrimaryKeyField { get; set; }
    }
}
