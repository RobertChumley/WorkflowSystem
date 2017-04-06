using System;
using System.Collections.Generic;
using System.Linq;
using WorkflowEngine.Workflow.Engine.WorkflowRules;
using WorkflowEngine.Workflow.Model.Parameters;

namespace WorkflowEngine.Workflow.Engine.WorkflowActions
{
    public static class WorkflowExecutionRuleExtensions
    {
        public static List<Func<WorkflowAction>> ExecuteRules(this List<WorkflowRule> rules,Func<string,WorkflowParameter> leftHandFunc)
        {
            return rules.Select((rule) => rule.Execute(leftHandFunc)).ToList();
        }
    }
}