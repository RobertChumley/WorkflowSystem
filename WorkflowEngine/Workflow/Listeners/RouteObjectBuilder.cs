using System;
using System.Collections.Generic;
using WorkflowEngine.Workflow.Engine.WorkflowContainer;
using WorkflowEngine.Workflow.Engine.WorkflowRegistries;

namespace WorkflowEngine.Workflow.Listeners
{
    public class RouteObjectBuilder
    {
        public RouteObjectBuilder()
        {
            WorkflowObjectTypeRegistry = RegistryContainer.Resolve<WorkflowObjectTypeRegistry>;
        }
        public Func<WorkflowObjectTypeRegistry> WorkflowObjectTypeRegistry { get; set; }
        public Func<Dictionary<string, object>,string,string,WorkflowDataObject> Initialize()
        {
            //string typeName,string primaryKeyValue
            return (fieldValues,typeName,primaryKeyValue) => new WorkflowDataObject(WorkflowObjectTypeRegistry()[typeName],fieldValues, primaryKeyValue);
        }
    }
}
