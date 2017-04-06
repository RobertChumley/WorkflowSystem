using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkflowEngine.Workflow.Engine.WorkflowStates;
using WorkflowEngine.Workflow.Model.Types;
using WorkflowEngine.Workflow.Model.WorkflowEvents;
using WorkflowEngine.Workflow.Model.WorkflowStates;

namespace WorkflowEngine.Workflow.Engine.WorkflowTransitions
{
    public class WorkflowTransition
    {
        public Func<WorkflowTransitionConfig> WorkstationTransitionConfig { get;  }
        public WorkflowTransition(Func<WorkflowTransitionConfig> workstationTransitionConfig)
        {
            WorkstationTransitionConfig = workstationTransitionConfig;
        }
    }
}
