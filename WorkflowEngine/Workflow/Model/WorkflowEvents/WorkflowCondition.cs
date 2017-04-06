using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WorkflowEngine.Workflow.Engine.WorkflowActions;
using WorkflowEngine.Workflow.Model.Parameters;

namespace WorkflowEngine.Workflow.Model.WorkflowEvents
{
    public class WorkflowConditionConfig
    {
        [Key]
        public int Id { get; set; }
        //Take the Value from the Conditional Paramter then execute the actions associated with the value.
        public WorkflowParameter ConditionalParamter { get; set; }
        public List<ConditionalOption> ConditionalActions { get; set; }
    }

    public class ConditionalOption
    {
        [Key]
        public int Id { get; set; }
        [NotMapped]
        public object OptionValue  { get; set; }
        public string WorkflowAction { get; set; }
    }

    
}