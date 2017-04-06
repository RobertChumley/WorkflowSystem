using System;
using System.Collections.Generic;
using WorkflowEngine.DatabaseEngine;
using WorkflowEngine.Workflow.Engine.WorkflowActions;
using WorkflowEngine.Workflow.Engine.WorkflowTransitions;
using WorkflowEngine.Workflow.ExecutionSystem;
using WorkflowEngine.Workflow.Listeners;
using WorkflowEngine.Workflow.Model.WorkflowActions;
using WorkflowEngine.Workflow.Model.WorkflowEvents;
using WorkflowEngine.Workflow.Model.WorkflowListeners;
using WorkflowEngine.Workflow.Model.WorkflowObjects;
using WorkflowEngine.Workflow.Model.WorkflowStates;

namespace WorkflowEngine.Workflow.Model
{
    public class WorkflowConfiguration
    { 
        public string WorkflowName { get; set; }
        public string InitialWorkflowStateName { get; set; }
        public List<WorkflowActionBaseConfig> WorkflowActions { get; set; }
        public List<WorkflowStateConfig> WorkflowStates { get; set; }
        public List<WorkflowTransitionConfig> WorkflowTransitions { get; set; }
        public List<WorkflowEventConfig> WorkflowEvents { get; set; }
        public List<WorkflowListenerConfigBase> WorkflowListeners { get; set; }
        public List<WorkflowObjectType> WorkflowObjectTypes { get; set; }
        public DatabaseConfig DatabaseConfig { get; set; }

        public Func<List<WorkflowAction>> BuildWorkflowActions(Func<WorkflowActionBaseConfig, WorkflowAction> buildAction)
        {
            return () => WorkflowActions.BuildWorkflowActions(buildAction);
        }
        public Func<List<Engine.WorkflowStates.WorkflowState>> BuildWorkflowStates(Func<WorkflowStateConfig, Engine.WorkflowStates.WorkflowState> buildState)
        {
            return () => WorkflowStates.BuildWorkflowStates(buildState);
        }
        public Func<List<WorkflowTransition>> BuildWorkflowTransitions(Func<WorkflowTransitionConfig, WorkflowTransition> buildTransition)
        {
            return () => WorkflowTransitions.BuildWorkflowTransitions(buildTransition);
        }
        public Func<List<WorkflowListenerBase>> BuildWorkflowListeners(Func<WorkflowListenerConfigBase, WorkflowListenerBase> buildListener)
        {
            return () => WorkflowListeners.BuildListeners(buildListener);
        }
    }
}