using System;
using System.Collections.Generic;
using System.Linq;
using WorkflowEngine.Workflow.Engine.WorkflowContainer;
using WorkflowEngine.Workflow.Engine.WorkflowRegistries;
using WorkflowEngine.Workflow.Model.Parameters;
using WorkflowEngine.Workflow.Model.WorkflowActions;

namespace WorkflowEngine.Workflow.Engine.WorkflowActions
{
    public class WorkflowConditionalAction : WorkflowAction
    {
        public WorkflowConditionalAction(Func<WorkflowActionBaseConfig> workflowActionConfiguration) : 
            base(workflowActionConfiguration) {}
        public Func<WorkflowAction> Execute()
        {
            var config =
                WorkflowCondition.BuildFromConfig((WorkflowConditionalActionConfig) WorkflowActionConfiguration()).CurrentAction();
            if (config != null)
                return config;
            return () => WorkflowActionRegistry()[WorkflowActionConfiguration().NextAction];
        }
    }
    public class WorkflowCondition
    {
        public WorkflowCondition(WorkflowParameter conditionalParamter, Dictionary<object, Func<WorkflowAction>> conditionalActions)
        {
            ConditionalParamter = conditionalParamter;
            ConditionalActions = conditionalActions;
        }
        public static WorkflowCondition BuildFromConfig(WorkflowConditionalActionConfig workflowConditionalActionConfig)
        {
            var conditionalActions = new Dictionary<object,Func<WorkflowAction>>();
            var registry = RegistryContainer.Resolve<WorkflowActionRegistry>();
            workflowConditionalActionConfig
                .WorkflowConditions
                .ConditionalActions
                .ForEach(condition=> 
                    conditionalActions[condition.OptionValue] = () => registry[condition.WorkflowAction]);
            return new WorkflowCondition(workflowConditionalActionConfig.WorkflowConditions.ConditionalParamter, conditionalActions);
        }

        public Func<WorkflowAction> CurrentAction()
        {
            var actionFunc = ConditionalActions.ContainsKey(ConditionalParamter.Value)
                   ? ConditionalActions[ConditionalParamter.Value] : null;
            return actionFunc;
        }
        //Take the Value from the Conditional Paramter then execute the actions associated with the value.
        public WorkflowParameter ConditionalParamter { get; set; }
        public Dictionary<object, Func<WorkflowAction>> ConditionalActions { get; set; }
    }
}