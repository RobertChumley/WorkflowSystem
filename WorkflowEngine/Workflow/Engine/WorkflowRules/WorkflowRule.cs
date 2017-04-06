using System;
using WorkflowEngine.Workflow.Engine.WorkflowActions;
using WorkflowEngine.Workflow.Engine.WorkflowRegistries;
using WorkflowEngine.Workflow.Model.Parameters;
using WorkflowEngine.Workflow.Model.Rules;
using WorkflowEngine.Workflow.Model.Types;

namespace WorkflowEngine.Workflow.Engine.WorkflowRules
{
    public class WorkflowRule
    {
        public WorkflowRule(Func<WorkflowRuleConfig> workflowRuleConfig)
        {
            WorkflowActionRegistry = WorkflowContainer.RegistryContainer.Resolve<WorkflowActionRegistry>;
            WorkflowRuleConfigFunc = workflowRuleConfig;

        }

        public Func<WorkflowRuleConfig> WorkflowRuleConfigFunc { get; }
        public Func<WorkflowActionRegistry> WorkflowActionRegistry { get;  }

        public Func<WorkflowAction> Execute(Func<string,WorkflowParameter> leftHandFunc)
        {
            var workflowRuleConfig = WorkflowRuleConfigFunc();
            switch (workflowRuleConfig.WorkflowRuleOperator)
            {
                 case WorkflowRuleOperator.Equals:
                    return () => leftHandFunc(workflowRuleConfig.LeftHandParam).Value == workflowRuleConfig.RightHand.Value 
                        ? WorkflowActionRegistry()[workflowRuleConfig.PositiveAction] : WorkflowActionRegistry()[workflowRuleConfig.ElseAction];
                case WorkflowRuleOperator.NotEquals:
                    return () => leftHandFunc(workflowRuleConfig.LeftHandParam).Value != workflowRuleConfig.RightHand.Value 
                        ? WorkflowActionRegistry()[workflowRuleConfig.PositiveAction] : WorkflowActionRegistry()[workflowRuleConfig.ElseAction];
                case WorkflowRuleOperator.GreaterThen:
                    return () => Convert.ToInt32(leftHandFunc(workflowRuleConfig.LeftHandParam).Value) > Convert.ToInt32(workflowRuleConfig.RightHand.Value) 
                        ? WorkflowActionRegistry()[workflowRuleConfig.PositiveAction] : WorkflowActionRegistry()[workflowRuleConfig.ElseAction];
                case WorkflowRuleOperator.LessThan:
                    return () => Convert.ToInt32(leftHandFunc(workflowRuleConfig.LeftHandParam).Value) < Convert.ToInt32(workflowRuleConfig.RightHand.Value) 
                        ? WorkflowActionRegistry()[workflowRuleConfig.PositiveAction] : WorkflowActionRegistry()[workflowRuleConfig.ElseAction];
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}