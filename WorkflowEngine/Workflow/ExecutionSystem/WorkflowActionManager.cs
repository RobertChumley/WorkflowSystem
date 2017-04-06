using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using WorkflowEngine.Workflow.Engine.WorkflowActions;
using WorkflowEngine.Workflow.Engine.WorkflowObjects;
using WorkflowEngine.Workflow.Model.Types;
using WorkflowEngine.Workflow.Model.WorkflowActions;
using WorkflowEngine.Workflow.Support;

namespace WorkflowEngine.Workflow.ExecutionSystem
{
    public class WorkflowActionManager
    {
        public WorkflowActionManager(Func<WorkflowStateManager> stateManager,Func<WorkflowDefinition> workflowDefinition)
        {
            StateManager = stateManager;
            WorkflowDefinition = workflowDefinition;
        }

        public Func<WorkflowDefinition> WorkflowDefinition { get; }
        public Func<WorkflowStateManager> StateManager { get; set; }

        public static WorkflowAction BuildAction(
            WorkflowActionBaseConfig config,
            Action<WorkflowStateManager> stateChangeNotifier )
        {
            switch (config.WorkflowActionType)
            {
                case WorkflowActionType.Transition:
                    return new WorkflowTransitionAction(() => config, stateChangeNotifier);
                case WorkflowActionType.Execute:
                    return new WorkflowExecuteStateRulesAction(() => config);
                case WorkflowActionType.Conditional:
                    return new WorkflowConditionalAction(() => config);
                case WorkflowActionType.Notification:
                    return new WorkflowNotificationAction(() => config);
                case WorkflowActionType.Listener:
                    return new WorkflowListenerAction(() => config, WorkflowEventType.Start);
                case WorkflowActionType.MutateState:
                    return new WorkflowMutateStateAction(() => config);
                case WorkflowActionType.Rule:
                    return new WorkflowRuleAction(() => config);
                case WorkflowActionType.Timer:
                    return new WorkflowTimerAction(() => config);
                case WorkflowActionType.Event:
                    return new WorkflowEventAction(() => config);
                case WorkflowActionType.Queue:
                    return new WorkflowQueueAction(() => config);
                case WorkflowActionType.SendSocket:
                    return new WorkflowSendSocketAction(() => config);
                default:
                    throw new ArgumentOutOfRangeException(nameof(config), config, null);
            }
        }

        public void ExecuteChain(Func<WorkflowAction> actionFunc)
        {
            var exec = actionFunc;
            while(exec != null)
            {
                exec = Execute(exec);
            }
        }
        public void ExecuteActions(List<Func<WorkflowAction>> workflowActions)
        {
            workflowActions.ForEach(ExecuteChain);
        }

        public Func<WorkflowAction> Execute(Func<WorkflowAction> actionFunc )
        {
            var action = actionFunc();
            if (action == null) return null;

            switch (action.WorkflowActionConfiguration().WorkflowActionType)
            {
                case WorkflowActionType.Execute:
                    return ExecuteAction(() => (WorkflowExecuteStateRulesAction)action);
                case WorkflowActionType.Conditional:
                    return ExecuteAction(() => (WorkflowConditionalAction)action);
                case WorkflowActionType.Transition:
                    return ExecuteAction(() => (WorkflowTransitionAction)action);
                case WorkflowActionType.Notification:
                    return ExecuteAction(() => (WorkflowNotificationAction)action);
                case WorkflowActionType.Listener:
                    return ExecuteAction(() => (WorkflowListenerAction)action);
                case WorkflowActionType.MutateState:
                    return ExecuteAction(() => (WorkflowMutateStateAction)action);
                case WorkflowActionType.Rule:
                    return ExecuteAction(() => (WorkflowRuleAction)action);
                case WorkflowActionType.Timer:
                    return ExecuteAction(() => (WorkflowTimerAction)action);
                case WorkflowActionType.Event:
                    return ExecuteAction(() => (WorkflowEventAction)action);
                case WorkflowActionType.Queue:
                    return ExecuteAction(() => (WorkflowQueueAction)action);
                case WorkflowActionType.SendSocket:
                    return ExecuteAction(() => (WorkflowSendSocketAction) action);
                default:
                    return () => null;
            }
        }

        private Func<WorkflowAction> ExecuteAction(Func<WorkflowQueueAction> workflowAction)
        {
            return workflowAction().Execute();
        }

        public Func<WorkflowAction> ExecuteAction(Func<WorkflowTransitionAction> workflowAction)
        {
            return workflowAction().Execute(StateManager, ((type) => WorkflowDefinition().RaiseEvent(type, new WorkflowDataObject())));
        }
        public Func<WorkflowAction> ExecuteAction(Func<WorkflowEventAction> workflowAction)
        {
            return workflowAction().Execute(((type) => WorkflowDefinition().RaiseEvent(type, new WorkflowDataObject())));
        }
        public Func<WorkflowAction> ExecuteAction(Func<WorkflowConditionalAction> workflowAction)
        {
            //return workflowAction().Execute(() => new List<WorkflowCondition>());
            return workflowAction().Execute();
        }
        public Func<WorkflowAction> ExecuteAction(Func<WorkflowTimerAction> workflowAction)
        {
            return workflowAction().Execute((action) => Task.Run(() => Execute(() => action)), new CancellationTokenSource());
        }
        public Func<WorkflowAction> ExecuteAction(Func<WorkflowNotificationAction> workflowAction)
        {
            return workflowAction().Execute();
        }
        public Func<WorkflowAction> ExecuteAction(Func<WorkflowListenerAction> workflowAction)
        {
            return workflowAction().Execute();
        }
        public Func<WorkflowAction> ExecuteAction(Func<WorkflowExecuteStateRulesAction> workflowAction)
        {
            return workflowAction().Execute(StateManager, ExecuteActions);
        }
        public Func<WorkflowAction> ExecuteAction(Func<WorkflowMutateStateAction> workflowAction)
        {
            return workflowAction().Execute(StateManager);
        }
        public Func<WorkflowAction> ExecuteAction(Func<WorkflowRuleAction> workflowAction)
        {
            return workflowAction().Execute();
        }
        public Func<WorkflowAction> ExecuteAction(Func<WorkflowSendSocketAction> workflowAction)
        {
            return workflowAction().Execute();
        }
    }
}
