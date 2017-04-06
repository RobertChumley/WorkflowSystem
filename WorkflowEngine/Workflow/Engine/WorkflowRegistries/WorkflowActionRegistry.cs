using System;
using System.Collections;
using System.Collections.Generic;

using WorkflowEngine.Workflow.Engine.WorkflowActions;
using WorkflowEngine.Workflow.Engine.WorkflowStates;
using WorkflowEngine.Workflow.Listeners;
using WorkflowEngine.Workflow.Model;
using WorkflowEngine.Workflow.Model.Types;
using WorkflowEngine.Workflow.Model.WorkflowObjects;

namespace WorkflowEngine.Workflow.Engine.WorkflowRegistries
{
    public abstract class BaseRegistry : DictionaryBase
    {
        
    }
    public class WorkflowActionRegistry : BaseRegistry
    {
        private readonly Dictionary<string, WorkflowAction> _registry;
        public WorkflowActionRegistry(Dictionary<string, WorkflowAction> registry)
        {
            _registry = registry;
        }
        public WorkflowAction this[string key] => key != null && _registry.ContainsKey(key) ? _registry[key] : null;
    }
    public class WorkflowObjectTypeRegistry : BaseRegistry
    {
        private readonly Dictionary<string, WorkflowObjectType> _registry;
        public WorkflowObjectTypeRegistry(Dictionary<string, WorkflowObjectType> registry)
        {
            _registry = registry;
        }
        public WorkflowObjectType this[string key] => key != null && _registry.ContainsKey(key) ? _registry[key] : null;
    }

    public class WorkflowListenerRegistry : BaseRegistry
    {
        private readonly Dictionary<string, WorkflowListenerBase> _registry;
        public WorkflowListenerRegistry(Dictionary<string, WorkflowListenerBase> registry)
        {
            _registry = registry;
        }
        public WorkflowListenerBase this[string key] => key != null && _registry.ContainsKey(key) ? _registry[key] : null;
    }
    public class WorkflowStateRegistry : BaseRegistry
    {
        private readonly Dictionary<string, WorkflowState> _registry;
        public WorkflowStateRegistry(Dictionary<string, WorkflowState> registry)
        {
            _registry = registry;
        }
        public WorkflowState this[string key] => key != null && _registry.ContainsKey(key) ? _registry[key] : null;
    }

    public class WorkflowEventRegistry : BaseRegistry
    {
        private readonly Dictionary<WorkflowEventType, WorkflowEventAction> _registry;
        public WorkflowEventRegistry(Dictionary<WorkflowEventType, WorkflowEventAction> registry)
        {
            _registry = registry;
        }

        public WorkflowEventAction this[WorkflowEventType key] => _registry.ContainsKey(key) ? _registry[key] : null;

        public bool ContainsKey(WorkflowEventType key)
        {
            return _registry.ContainsKey(key);
        }
    }
    
}
