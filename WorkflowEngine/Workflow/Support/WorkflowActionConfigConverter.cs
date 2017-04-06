using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using WorkflowEngine.Workflow.Model.Types;
using WorkflowEngine.Workflow.Model.WorkflowActions;
using WorkflowEngine.Workflow.Model.WorkflowListeners;

namespace WorkflowEngine.Workflow.Support
{
    public class WorkflowActionConfigConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return typeof(WorkflowActionBaseConfig).IsAssignableFrom(objectType) || typeof(WorkflowListenerConfigBase).IsAssignableFrom(objectType);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var item = JObject.Load(reader);

            if (typeof(WorkflowActionBaseConfig).IsAssignableFrom(objectType))
                return ConvertActionConfig(item);
            if (typeof(WorkflowListenerConfigBase).IsAssignableFrom(objectType))
                return ConvertListenerConfig(item);
            return item;
        }

        private object ConvertListenerConfig(JObject item)
        {
            ListenerType listenerType;
            if (!Enum.TryParse(item["ListenerType"].Value<string>(), out listenerType))
            {
                throw new Exception("Cannot parse type");
            }
            switch (listenerType)
            {
                case ListenerType.WebAPI:
                    return item.ToObject<WorkflowWebApiListenerConfig>();
                case ListenerType.Queue:
                    break;
                case ListenerType.WebSocket:
                    return item.ToObject<WorkflowWebSocketListenerConfig>();
                case ListenerType.SignalR:
                    break;
                case ListenerType.FileSystem:
                    break;
                case ListenerType.AWSS3:
                    break;
                case ListenerType.AzureBlob:
                    break;
                case ListenerType.Socket:
                    return item.ToObject<WorkflowSocketListenerConfig>();
                default:
                    throw new ArgumentOutOfRangeException();
            }
            return null;
        }

        private object ConvertActionConfig(JObject item)
        {
            WorkflowActionType theType;
            if (!Enum.TryParse(item["WorkflowActionType"].Value<string>(), out theType))
            {
                throw new Exception("Cannot parse type");
            }
            switch (theType)
            {
                case WorkflowActionType.Execute:
                case WorkflowActionType.Rule:
                case WorkflowActionType.Notification:
                case WorkflowActionType.Queue:
                    return item.ToObject<WorkflowQueueActionConfig>();
                case WorkflowActionType.Listener:
                    return item.ToObject<WorkflowActionConfig>();
                case WorkflowActionType.Conditional:
                    return item.ToObject<WorkflowConditionalActionConfig>();
                case WorkflowActionType.MutateState:
                    return item.ToObject<WorkflowMutateActionConfig>();
                case WorkflowActionType.Timer:
                    return item.ToObject<WorkflowTimerActionConfig>();
                case WorkflowActionType.Transition:
                    return item.ToObject<WorkflowTransitionActionConfig>();
                case WorkflowActionType.Event:
                    return item.ToObject<WorkflowEventActionConfig>();
                case WorkflowActionType.SendSocket:
                    return item.ToObject<WorkflowSendSocketActionConfig>();
                default:
                    return item.ToObject<WorkflowActionConfig>();
            }
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}