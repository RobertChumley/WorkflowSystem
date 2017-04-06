using Microsoft.EntityFrameworkCore;
using WorkflowEngine.Workflow.Listeners;
using WorkflowEngine.Workflow.Model.WorkflowActions;
using WorkflowEngine.Workflow.Model.WorkflowListeners;
using WorkflowEngine.Workflow.Model.WorkflowObjects;
using WorkflowEngine.Workflow.Model.WorkflowStates;

namespace FormsModel
{
    public class WorkflowContext :DbContext
    {

        public WorkflowContext(DbContextOptions<WorkflowContext> options)
            : base(options)
        { }

        public DbSet<WorkflowStateConfig> WorkflowStateConfigs { get; set; }
        public DbSet<WorkflowListenerConfigBase> WebApiListeners { get; set; }
        public DbSet<WorkflowActionBaseConfig> WorkflowActionConfigs { get; set; }
        public DbSet<WorkflowObjectType> WorkflowObjectTypes { get; set; }
        public DbSet<WorkflowConditionalActionConfig> WorkflowConditionalActionConfigs { get;set;}
        public DbSet<WorkflowEventActionConfig> WorkflowEventActionConfigs { get;set;}    
        public DbSet<WorkflowMutateActionConfig> WorkflowMutateActionConfigs { get;set;}
        public DbSet<WorkflowQueueActionConfig> WorkflowQueueActionConfigs { get;set;}
        public DbSet<WorkflowTimerActionConfig> WorkflowTimerActionConfigs { get;set;}
        public DbSet<WorkflowTransitionActionConfig> WorkflowTransitionActionConfigs { get;set;}
        public DbSet<WorkflowWebApiListenerConfig> WorkflowWebAPIListenerConfigs { get; set; }
    }
}
