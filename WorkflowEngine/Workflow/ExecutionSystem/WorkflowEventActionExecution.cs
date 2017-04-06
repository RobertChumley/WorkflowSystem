using System;
using System.Collections.Generic;
using System.Xml;
using WorkflowEngine.Workflow.Engine.WorkflowActions;
using WorkflowEngine.Workflow.Engine.WorkflowContainer;
using WorkflowEngine.Workflow.Engine.WorkflowObjects;
using WorkflowEngine.Workflow.Engine.WorkflowRegistries;
using WorkflowEngine.Workflow.Model.Types;

namespace WorkflowEngine.Workflow.ExecutionSystem
{
    public class WorkflowEventActionExecution
    {
        
        public WorkflowEventActionExecution()
        {
            WorkflowActionRegistry = RegistryContainer.Resolve<WorkflowActionRegistry>;
            WorkflowEventRegistry = RegistryContainer.Resolve<WorkflowEventRegistry>;
            WorkflowActionManager = RegistryContainer.Resolve<WorkflowActionManager>;
        }

        public Func<WorkflowEventRegistry> WorkflowEventRegistry { get; }
        public Func<WorkflowActionRegistry> WorkflowActionRegistry { get; }
        public Func<WorkflowActionManager> WorkflowActionManager { get; }

        public void ExecuteAction(Func<WorkflowActionRegistry, WorkflowAction> eventFunc)
        {
            WorkflowActionManager().ExecuteChain(() => eventFunc(WorkflowActionRegistry()));
        }
        public void RaiseEvent(WorkflowEventType workflowEventType, WorkflowDataObject workflowDataObject)
        {
            if (!WorkflowEventRegistry().ContainsKey(workflowEventType)) return;
            WorkflowActionManager().ExecuteChain(() => 
                WorkflowActionRegistry()[WorkflowEventRegistry()[workflowEventType].WorkflowEventActionConfig.WorkflowActionName]);
        }
    }
}
