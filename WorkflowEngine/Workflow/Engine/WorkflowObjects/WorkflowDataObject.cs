using System.Collections.Generic;
using WorkflowEngine.Workflow.Engine.WorkflowStates;

namespace WorkflowEngine.Workflow.Engine.WorkflowObjects
{
    public class WorkflowDataObject
    {
        //ToDO: Some incarnation of the workflow associated with this object
        //Imutable?  Means that the fundamental object changes when it changes states
        public Dictionary<string,object> WorkflowProperties { get; set; }
        public WorkflowState CurrentWorkflowState { get; set; }
    }
    
}
