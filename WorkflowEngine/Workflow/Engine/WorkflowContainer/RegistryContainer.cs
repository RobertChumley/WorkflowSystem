using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace WorkflowEngine.Workflow.Engine.WorkflowContainer
{
    public class RegistryContainer
    {
        private static WorkflowContainer _workflowContainer;

        public static WorkflowContainer Instance()
        {
            return _workflowContainer ?? (_workflowContainer = new WorkflowContainer());
        }

        public static T Resolve<T>() where T : class
        {
            return Instance().HasKey(typeof(T)) ? (T)_workflowContainer[typeof(T)] : null;
        }

        public static void Register<T>(object item)
        {
            Instance()[typeof(T)] = item;
        }
    }

    public class WorkflowContainer : DictionaryBase
    {
        private readonly Dictionary<Type, object> _workflowContainer;

        public WorkflowContainer()
        {
            _workflowContainer = new Dictionary<Type, object>();
        }
        public object this[Type key]
        {
            get { return _workflowContainer[key]; }
            set { _workflowContainer[key] = value; }
        }

        public bool HasKey(Type type)
        {
            return _workflowContainer.ContainsKey(type);
        }
    }
}
