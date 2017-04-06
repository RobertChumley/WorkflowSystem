using System;
using WorkflowEngine.Workflow.Model.Types;

namespace WorkflowEngine.Workflow.Model.WorkflowActions
{
    public class WorkflowTimerActionConfig : WorkflowActionBaseConfig
    {
        public DateTime? Start { get; set; }
        public DateTime? End { get; set; }
        public int Tick { get; set; }
        public TickType TickType { get; set; }
        public DateTime? LastExecuted { get; set; }
        public int Iterations { get; set; }
        public string TimerAction { get; set; }
    }
}