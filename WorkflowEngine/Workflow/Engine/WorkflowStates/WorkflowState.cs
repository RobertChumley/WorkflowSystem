using System;
using System.Collections.Generic;
using System.Linq;
using WorkflowEngine.Workflow.Engine.WorkflowActions;
using WorkflowEngine.Workflow.Engine.WorkflowRules;
using WorkflowEngine.Workflow.Model.Parameters;
using WorkflowEngine.Workflow.Model.Types;
using WorkflowEngine.Workflow.Model.WorkflowStates;

namespace WorkflowEngine.Workflow.Engine.WorkflowStates
{
    public class WorkflowState
    {
        public WorkflowState(Func<WorkflowStateConfig> workflowStateConfig )
        {
            WorkflowStateConfiguration = workflowStateConfig();
            CurrentParameterValues = new Dictionary<string, WorkflowParameter>();
            WorkflowStateConfiguration.WorkflowParameters.ForEach(param => CurrentParameterValues[param.Name] = param);
            WorkflowExecutionRules = WorkflowStateConfiguration.WorkflowExecutionRules.Select(i => new WorkflowRule(() => i)).ToList();
        }

        public WorkflowStateConfig WorkflowStateConfiguration { get; }
        public Dictionary<string, WorkflowParameter> CurrentParameterValues { get;  }
        public List<WorkflowRule> WorkflowExecutionRules { get;  }
        
        public Action<WorkflowParameter> Mutate(MutationOperatorType operatorType)
        {
            switch (operatorType)
            {
                case MutationOperatorType.Increment:
                    return (parameter) => CurrentParameterValues[parameter.Name].Value = (Convert.ToInt32(CurrentParameterValues[parameter.Name].Value) + Convert.ToInt32(parameter.Value)).ToString();
                case MutationOperatorType.Degrement:
                    return (parameter) => CurrentParameterValues[parameter.Name].Value = (Convert.ToInt32(CurrentParameterValues[parameter.Name].Value) - Convert.ToInt32(parameter.Value)).ToString();
                case MutationOperatorType.SetTo:
                    return (parameter) => CurrentParameterValues[parameter.Name].Value = parameter.Value;
                default:
                    throw new ArgumentOutOfRangeException(nameof(operatorType), operatorType, null);
            }
            
        }

        public void ExecuteState()
        {
            WorkflowExecutionRules.ExecuteRules((paramName) => CurrentParameterValues[paramName]);
        }
    }
}