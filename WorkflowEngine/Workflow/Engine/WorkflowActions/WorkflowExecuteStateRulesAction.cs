using System;
using System.Collections.Generic;
using System.Linq;
using WorkflowEngine.Workflow.ExecutionSystem;
using WorkflowEngine.Workflow.Model.WorkflowActions;
using WorkflowEngine.Workflow.Support;

namespace WorkflowEngine.Workflow.Engine.WorkflowActions
{
    public class WorkflowExecuteStateRulesAction : WorkflowAction
    {
        public WorkflowExecuteStateRulesAction(Func<WorkflowActionBaseConfig> workflowActionConfiguration) : base(workflowActionConfiguration) {}
        
        public Func<WorkflowAction> Execute(Func<WorkflowStateManager> workflowStateManager,Action<List<Func<WorkflowAction>>> actionManager )
        {
            actionManager(
                workflowStateManager()
                    .CurrentWorkflowState
                    .WorkflowExecutionRules.ExecuteRules((paramName) => 
                        workflowStateManager()
                        .CurrentWorkflowState
                        .CurrentParameterValues[paramName])
                        .Where(i=>i != null).ToList());
            return () => WorkflowActionRegistry()[WorkflowActionConfiguration().NextAction];
        }
    }
}