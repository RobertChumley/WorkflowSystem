using System.ComponentModel.DataAnnotations;

namespace WorkflowEngine.Workflow.Model.WorkflowObjects
{
    public class ObjectField
    {
        [Key]
        public int Id { get; set; }

        public string FieldName { get; set; }
        public string FieldDisplay { get; set; }
        public string FieldType { get; set; }

        public string MappedDBType()
        {
            switch (FieldType)
            {
                case "int":
                    return "int";
                case "bool":
                    return "bit";
                case "string":
                    return "nvarchar(4000)";
                case "decimal":
                    return "decimal(29,4)";
                case "long":
                    return "bigint";
                case "double":
                    return "float";
            }
            return "nvarchar(max)";
        }
    }
}