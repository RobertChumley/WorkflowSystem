using System;
using System.Collections.Generic;
using WorkflowEngine.Workflow.Engine.WorkflowRules;
using WorkflowEngine.Workflow.Model.Parameters;
using WorkflowEngine.Workflow.Model.Rules;
using WorkflowEngine.Workflow.Model.WorkflowActions;

namespace WorkflowEngine.Workflow.Engine.WorkflowActions
{
    public class WorkflowRuleAction : WorkflowAction
    {
        public List<WorkflowRule> WorkflowRules { get; set; }

        public WorkflowRuleAction(Func<WorkflowActionBaseConfig> workflowActionConfiguration) : base(workflowActionConfiguration)
        {
        }

        public Func<WorkflowAction> Execute()
        {
            //ToDo: This does not work because this needs to know where the param is coming from, also what do we do with the returned actions
            WorkflowRules.ExecuteRules((paramName)=>new WorkflowParameter()).ForEach(rule => rule());
            return () => WorkflowActionRegistry()[WorkflowActionConfiguration().NextAction];
        }
    }
}