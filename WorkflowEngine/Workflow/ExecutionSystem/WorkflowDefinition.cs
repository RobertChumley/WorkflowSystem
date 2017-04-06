using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WorkflowEngine.DatabaseEngine;
using WorkflowEngine.Workflow.Engine.WorkflowActions;
using WorkflowEngine.Workflow.Engine.WorkflowContainer;
using WorkflowEngine.Workflow.Engine.WorkflowObjects;
using WorkflowEngine.Workflow.Engine.WorkflowRegistries;
using WorkflowEngine.Workflow.Engine.WorkflowStates;
using WorkflowEngine.Workflow.Engine.WorkflowTransitions;
using WorkflowEngine.Workflow.Listeners;
using WorkflowEngine.Workflow.Model;
using WorkflowEngine.Workflow.Model.Types;
using WorkflowEngine.Workflow.Model.WorkflowActions;
using WorkflowEngine.Workflow.Model.WorkflowEvents;
using WorkflowEngine.Workflow.Model.WorkflowListeners;
using WorkflowEngine.Workflow.Model.WorkflowObjects;
using WorkflowEngine.Workflow.Model.WorkflowStates;
using WorkflowEngine.Workflow.Support;
using WorkflowDataObject = WorkflowEngine.Workflow.Engine.WorkflowObjects.WorkflowDataObject;

namespace WorkflowEngine.Workflow.ExecutionSystem
{

    public class WorkflowDefinition
    {
        public WorkflowDefinition(WorkflowConfiguration workflowConfiguration)
        {
            WorkflowName = workflowConfiguration.WorkflowName;
            InitialWorkflowStateName = workflowConfiguration.InitialWorkflowStateName;
            WorkflowActions = workflowConfiguration.BuildWorkflowActions((item)=>
                WorkflowActionManager.BuildAction(item,SetNewManager))()
                .ToActionDictioary();
            WorkflowStates = workflowConfiguration.BuildWorkflowStates(WorkflowStateManager.BuildState)().ToStateDictionary();
            WorkflowListeners = workflowConfiguration.BuildWorkflowListeners(WorkflowListenerManager.BuildListener)().ToListenerDictionary();
            WorkflowTransitions = workflowConfiguration.BuildWorkflowTransitions(WorkflowTransitionManager.BuildTransition)();
            WorkflowEventActions = workflowConfiguration.WorkflowEvents.ToEventDefinition(WorkflowActions);
            WorkflowObjectTypes = workflowConfiguration.WorkflowObjectTypes.ToObjectTypeDictionary();
            DatabaseConfig = workflowConfiguration.DatabaseConfig;
        }

        public string InitialWorkflowStateName { get;  }
        public string WorkflowName { get;  }
        public DatabaseConfig DatabaseConfig { get; set; }
        public Dictionary<string, WorkflowListenerBase> WorkflowListeners { get; }
        public Dictionary<string, WorkflowAction> WorkflowActions { get;  }
        public Dictionary<string, WorkflowState> WorkflowStates { get;  }
        public Dictionary<string, WorkflowObjectType> WorkflowObjectTypes { get; }
        public List<WorkflowTransition> WorkflowTransitions { get;  }
        public Dictionary<WorkflowEventType, WorkflowEventAction> WorkflowEventActions { get; }
        
        public WorkflowDefinition RegisterTypes()
        {
            RegistryContainer.Register<WorkflowActionRegistry>(new WorkflowActionRegistry(WorkflowActions));
            RegistryContainer.Register<WorkflowStateRegistry>(new WorkflowStateRegistry(WorkflowStates));
            RegistryContainer.Register<WorkflowEventRegistry>(new WorkflowEventRegistry(WorkflowEventActions));
            RegistryContainer.Register<WorkflowListenerRegistry>(new WorkflowListenerRegistry(WorkflowListeners));
            RegistryContainer.Register<WorkflowObjectTypeRegistry>(new WorkflowObjectTypeRegistry(WorkflowObjectTypes));
            RegistryContainer.Register<WorkflowDatabaseEngine>(new WorkflowDatabaseEngine(() => new WorkflowSQLConnection(DatabaseConfig)));
            RegistryContainer.Register<WorkflowDefinition>(this);
            return this;
        }

        private void SetNewManager(WorkflowStateManager manager)
        {
            RegistryContainer.Register<WorkflowActionManager>(
               new WorkflowActionManager(() => manager, () => this));
        }

        public Func<WorkflowDefinition> RaiseEvent(WorkflowEventType eventType, WorkflowDataObject workflowDataObject)
        {
            return () =>
            {
                new WorkflowEventActionExecution().RaiseEvent(eventType, workflowDataObject);
                return this;
            };
        }

        public Func<WorkflowDefinition> ExecuteCustomEvent(Func<WorkflowActionRegistry, WorkflowAction> registryAction)
        {
            return () =>
            {
                new WorkflowEventActionExecution().ExecuteAction(registryAction);
                return this;
            };
        }
        public WorkflowDefinition FindAction(Action<WorkflowActionRegistry> registryAction)
        {
            registryAction(RegistryContainer.Resolve<WorkflowActionRegistry>());
            return this;
        }
        public WorkflowDefinition BuildActionManager()
        {
            
            RegistryContainer.Register<WorkflowActionManager>(
                new WorkflowActionManager(() => 
                    new WorkflowStateManager(WorkflowStates[InitialWorkflowStateName]),
                ()=>this));
            return this;
        }

        public WorkflowDefinition BuildSqlTables()
        {
            new DatabaseTableBuilder(DatabaseConfig).BuildSQLTables(WorkflowObjectTypes);
            return this;
        }
        public WorkflowActionManager CurrentActionManager()
        {
            return RegistryContainer.Resolve<WorkflowActionManager>();
        }

        public WorkflowDefinition CauseDelay(int i)
        {
            Task.Delay(i*1000).ConfigureAwait(false);

            return this;
        }

        public WorkflowDefinition RegisterListeners()
        {
            Console.WriteLine("Starting Raw sockets");
            WorkflowListeners.Keys
                .Where(i=> WorkflowListeners[i].WorkflowListenerConfig.ListenerType == ListenerType.Socket).ToList().ForEach(
                listener =>
                {
                    Task.Run(() => ((WorkflowSocketListener)WorkflowListeners[listener]).RegisterListener()).ConfigureAwait(false) ;
                    //((WorkflowSocketListener)listener).RegisterListener()();
                });
            Console.WriteLine("Starting web sockets");
            WorkflowListeners.Keys
                .Where(i => WorkflowListeners[i].WorkflowListenerConfig.ListenerType == ListenerType.WebSocket).ToList().ForEach(
                listener =>
                {
                    Console.WriteLine($"starting {listener}");
                    Task.Run(() => ((WorkflowWebSocketListener)WorkflowListeners[listener]).RegisterListener()()).ConfigureAwait(false);
                    //((WorkflowSocketListener)listener).RegisterListener()();
                });
            return this;
        }
    }

    public static class WorkflowListExtensions
    {
        public static Dictionary<WorkflowEventType, WorkflowEventAction> ToEventDefinition(
            this List<WorkflowEventConfig> eventConfigs,Dictionary<string,WorkflowAction> actions)
        {
            var ret = new Dictionary<WorkflowEventType, WorkflowEventAction>();
            eventConfigs.ForEach(eventConfig => 
                ret[eventConfig.WorkflowEventType] = (WorkflowEventAction)actions[eventConfig.WorkflowActionName]);
            return ret;
        }

        public static Dictionary<string, WorkflowObjectType> ToObjectTypeDictionary(
            this List<WorkflowObjectType> workflowObjectTypes)
        {
            return workflowObjectTypes.ToDictionary(i => i.ObjectTypeName, (i) => i);
        }
        public static Dictionary<string, WorkflowListenerBase> ToListenerDictionary(this List<WorkflowListenerBase> workflowListeners)
        {
            return workflowListeners.ToDictionary((i) => i.WorkflowListenerConfig.WorkflowListenerName, (i) => i);
        }
        public static Dictionary<string, WorkflowState> ToStateDictionary(this List<WorkflowState> workflowStates)
        {
            return workflowStates.ToDictionary(i => i.WorkflowStateConfiguration.StateName, i => i);
        }
        public static Dictionary<string, WorkflowAction> ToActionDictioary(this List<WorkflowAction> workflowStates)
        {
            return workflowStates.ToDictionary(i=>i.WorkflowActionConfiguration().WorkflowActionName,i=>i);
        }
        public static List<WorkflowAction> BuildWorkflowActions(this List<WorkflowActionBaseConfig> workflowActionConfigs,
            Func<WorkflowActionBaseConfig, WorkflowAction> workflowActionManagerFunc)
        {
            return workflowActionConfigs.Select(workflowActionManagerFunc).ToList();
        }

        public static List<WorkflowState> BuildWorkflowStates(this List<WorkflowStateConfig> stateConfigs,
            Func<WorkflowStateConfig, WorkflowState> workflowStateFunc)
        {
            return stateConfigs.Select(workflowStateFunc).ToList();
        }

        public static List<WorkflowTransition> BuildWorkflowTransitions(
            this List<WorkflowTransitionConfig> workflowTransitionConfig,
            Func<WorkflowTransitionConfig, WorkflowTransition> workflowTransitionFunc)
        {
            return workflowTransitionConfig.Select(workflowTransitionFunc).ToList();
        }

        public static List<WorkflowListenerBase> BuildListeners(
            this List<WorkflowListenerConfigBase> listeners, Func<WorkflowListenerConfigBase,WorkflowListenerBase> workflowListenerFunc)
        {
            return listeners.Select(workflowListenerFunc).ToList();
        }
    }
}
