using System;
using WorkflowEngine.Workflow.Engine.WorkflowObjects;
using WorkflowEngine.Workflow.Engine.WorkflowTransitions;
using WorkflowEngine.Workflow.ExecutionSystem;
using WorkflowEngine.Workflow.Model;
using WorkflowEngine.Workflow.Model.Types;
using WorkflowEngine.Workflow.Support;

namespace WorkflowEngine
{
    public class WorkflowController
    {
       
        public Func<WorkflowConfiguration,WorkflowDefinition> IntializeWorkflow(WorkflowDataObject workflowDataObject)
        {
            
           return  (workflowConfig) => new WorkflowDefinition(workflowConfig)
            .RegisterTypes()
            .BuildActionManager()
            .RegisterListeners()
            .RaiseEvent(WorkflowEventType.Start, workflowDataObject)();

            //Check for errors
            //Should write to the logs
            
            //What ways can a workflow transition / execute.... We know rules can execute... Timers can execute, inbound listeners can execute..
            //Workflow is executed on a per object bassis, so we will always be starting a workflow based on an inboud object...
            //That inbound object must be defined..  Has properties and methods.

            // A workflow is a dicodomy of the states of an object along with the history of actions and the 
            //series of events that can be executed because, on behalf or as a result of.  A workflow is also based on the world around an object.  
            //It can also spawn objects with workflows, so a workflow object as its basis is that thing that has the workflow applied
        }
        public void ExecuteWorkflow(WorkflowDefinition definition, WorkflowDataObject workflowDataObject)
        {

            workflowDataObject.CurrentWorkflowState.ExecuteState();
        }
        
    }

    
    public class WorkflowConfig
    {
    }
}
