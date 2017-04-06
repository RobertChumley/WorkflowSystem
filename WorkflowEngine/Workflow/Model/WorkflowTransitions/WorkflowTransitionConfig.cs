using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkflowEngine.Workflow.Model.WorkflowEvents;

namespace WorkflowEngine.Workflow.Model.WorkflowTransitions
{
    public class WorkflowTransitionConfig
    {

        public string TransitionReason { get; set; }
        public string SourceState { get; set; }
        public string DestinationState { get; set; }
        public string OnTransitionInEventName { get; set; }
        public string OnTransitionOutEventName { get; set; }
    }
}
