using System;
using WorkflowEngine.Workflow.Engine.WorkflowContainer;
using WorkflowEngine.Workflow.Engine.WorkflowRegistries;
using WorkflowEngine.Workflow.Model.WorkflowActions;

namespace WorkflowEngine.Workflow.Engine.WorkflowActions
{
    public abstract class WorkflowAction
    {
        protected WorkflowAction(Func<WorkflowActionBaseConfig> workflowActionConfiguration)
        {
            WorkflowActionConfiguration = workflowActionConfiguration;
            WorkflowActionRegistry = RegistryContainer.Resolve<WorkflowActionRegistry>;
        }
        public Func<WorkflowActionBaseConfig> WorkflowActionConfiguration { get; }
        public Func<WorkflowActionRegistry> WorkflowActionRegistry { get; }

        
    }
}