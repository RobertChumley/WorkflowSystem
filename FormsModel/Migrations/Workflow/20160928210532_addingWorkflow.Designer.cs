using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using FormsModel;

namespace FormsModel.Migrations.Workflow
{
    [DbContext(typeof(WorkflowContext))]
    [Migration("20160928210532_addingWorkflow")]
    partial class addingWorkflow
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.0.1")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("WorkflowEngine.Workflow.Engine.WorkflowActions.ServerConfig", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Url");

                    b.Property<int?>("WorkflowQueueActionConfigId");

                    b.HasKey("Id");

                    b.HasIndex("WorkflowQueueActionConfigId");

                    b.ToTable("ServerConfig");
                });

            modelBuilder.Entity("WorkflowEngine.Workflow.Model.Parameters.WorkflowParameter", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name");

                    b.Property<string>("Type");

                    b.Property<int?>("WorkflowActionBaseConfigId");

                    b.Property<int?>("WorkflowActionBaseConfigId1");

                    b.Property<int?>("WorkflowStateConfigId");

                    b.HasKey("Id");

                    b.HasIndex("WorkflowActionBaseConfigId");

                    b.HasIndex("WorkflowActionBaseConfigId1");

                    b.HasIndex("WorkflowStateConfigId");

                    b.ToTable("WorkflowParameter");
                });

            modelBuilder.Entity("WorkflowEngine.Workflow.Model.Rules.WorkflowRuleConfig", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ElseAction");

                    b.Property<int?>("LeftHandId");

                    b.Property<string>("LeftHandParam");

                    b.Property<string>("PositiveAction");

                    b.Property<int?>("RightHandId");

                    b.Property<int>("WorkflowRuleOperator");

                    b.Property<int?>("WorkflowStateConfigId");

                    b.HasKey("Id");

                    b.HasIndex("LeftHandId");

                    b.HasIndex("RightHandId");

                    b.HasIndex("WorkflowStateConfigId");

                    b.ToTable("WorkflowRuleConfig");
                });

            modelBuilder.Entity("WorkflowEngine.Workflow.Model.WorkflowActions.WorkflowActionBaseConfig", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Discriminator")
                        .IsRequired();

                    b.Property<string>("NextAction");

                    b.Property<string>("WorkflowActionName");

                    b.Property<int>("WorkflowActionType");

                    b.Property<int?>("WorkflowRoutineId");

                    b.HasKey("Id");

                    b.HasIndex("WorkflowRoutineId");

                    b.ToTable("WorkflowActionConfigs");

                    b.HasDiscriminator<string>("Discriminator").HasValue("WorkflowActionBaseConfig");
                });

            modelBuilder.Entity("WorkflowEngine.Workflow.Model.WorkflowActions.WorkflowRoutine", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("MethodName");

                    b.Property<string>("TypeName");

                    b.HasKey("Id");

                    b.ToTable("WorkflowRoutine");
                });

            modelBuilder.Entity("WorkflowEngine.Workflow.Model.WorkflowEvents.ConditionalOption", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("WorkflowAction");

                    b.Property<int?>("WorkflowConditionConfigId");

                    b.HasKey("Id");

                    b.HasIndex("WorkflowConditionConfigId");

                    b.ToTable("ConditionalOption");
                });

            modelBuilder.Entity("WorkflowEngine.Workflow.Model.WorkflowEvents.WorkflowConditionConfig", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int?>("ConditionalParamterId");

                    b.HasKey("Id");

                    b.HasIndex("ConditionalParamterId");

                    b.ToTable("WorkflowConditionConfig");
                });

            modelBuilder.Entity("WorkflowEngine.Workflow.Model.WorkflowListeners.ListenerActionMap", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ListenerAction");

                    b.Property<string>("ListenerMethod");

                    b.Property<int?>("WorkflowListenerConfigBaseId");

                    b.HasKey("Id");

                    b.HasIndex("WorkflowListenerConfigBaseId");

                    b.ToTable("ListenerActionMap");
                });

            modelBuilder.Entity("WorkflowEngine.Workflow.Model.WorkflowListeners.WorkflowListenerConfigBase", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Discriminator")
                        .IsRequired();

                    b.Property<int>("ListenerType");

                    b.Property<string>("WorkflowObjectName");

                    b.HasKey("Id");

                    b.ToTable("WebApiListeners");

                    b.HasDiscriminator<string>("Discriminator").HasValue("WorkflowListenerConfigBase");
                });

            modelBuilder.Entity("WorkflowEngine.Workflow.Model.WorkflowObjects.ObjectField", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("FieldDisplay");

                    b.Property<string>("FieldName");

                    b.Property<string>("FieldType");

                    b.Property<int?>("WorkflowObjectTypeId");

                    b.HasKey("Id");

                    b.HasIndex("WorkflowObjectTypeId");

                    b.ToTable("ObjectField");
                });

            modelBuilder.Entity("WorkflowEngine.Workflow.Model.WorkflowObjects.WorkflowObjectType", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ObjectTypeName");

                    b.Property<string>("PrimaryKeyField");

                    b.HasKey("Id");

                    b.ToTable("WorkflowObjectTypes");
                });

            modelBuilder.Entity("WorkflowEngine.Workflow.Model.WorkflowStates.WorkflowStateConfig", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("StateName");

                    b.Property<int>("WorkflowStateType");

                    b.HasKey("Id");

                    b.ToTable("WorkflowStateConfigs");
                });

            modelBuilder.Entity("WorkflowEngine.Workflow.Model.WorkflowActions.WorkflowConditionalActionConfig", b =>
                {
                    b.HasBaseType("WorkflowEngine.Workflow.Model.WorkflowActions.WorkflowActionBaseConfig");

                    b.Property<int?>("WorkflowConditionsId");

                    b.HasIndex("WorkflowConditionsId");

                    b.ToTable("WorkflowConditionalActionConfig");

                    b.HasDiscriminator().HasValue("WorkflowConditionalActionConfig");
                });

            modelBuilder.Entity("WorkflowEngine.Workflow.Model.WorkflowActions.WorkflowEventActionConfig", b =>
                {
                    b.HasBaseType("WorkflowEngine.Workflow.Model.WorkflowActions.WorkflowActionBaseConfig");

                    b.Property<string>("EventName");

                    b.Property<int>("WorkflowEventType");

                    b.ToTable("WorkflowEventActionConfig");

                    b.HasDiscriminator().HasValue("WorkflowEventActionConfig");
                });

            modelBuilder.Entity("WorkflowEngine.Workflow.Model.WorkflowActions.WorkflowMutateActionConfig", b =>
                {
                    b.HasBaseType("WorkflowEngine.Workflow.Model.WorkflowActions.WorkflowActionBaseConfig");

                    b.Property<int>("MutationOperatorType");

                    b.Property<int?>("MutationParameterId");

                    b.HasIndex("MutationParameterId");

                    b.ToTable("WorkflowMutateActionConfig");

                    b.HasDiscriminator().HasValue("WorkflowMutateActionConfig");
                });

            modelBuilder.Entity("WorkflowEngine.Workflow.Model.WorkflowActions.WorkflowQueueActionConfig", b =>
                {
                    b.HasBaseType("WorkflowEngine.Workflow.Model.WorkflowActions.WorkflowActionBaseConfig");

                    b.Property<string>("QueueAction");

                    b.Property<string>("QueueName");

                    b.Property<int>("QueueType");

                    b.ToTable("WorkflowQueueActionConfig");

                    b.HasDiscriminator().HasValue("WorkflowQueueActionConfig");
                });

            modelBuilder.Entity("WorkflowEngine.Workflow.Model.WorkflowActions.WorkflowTimerActionConfig", b =>
                {
                    b.HasBaseType("WorkflowEngine.Workflow.Model.WorkflowActions.WorkflowActionBaseConfig");

                    b.Property<DateTime?>("End");

                    b.Property<int>("Iterations");

                    b.Property<DateTime?>("LastExecuted");

                    b.Property<DateTime?>("Start");

                    b.Property<int>("Tick");

                    b.Property<int>("TickType");

                    b.Property<string>("TimerAction");

                    b.ToTable("WorkflowTimerActionConfig");

                    b.HasDiscriminator().HasValue("WorkflowTimerActionConfig");
                });

            modelBuilder.Entity("WorkflowEngine.Workflow.Model.WorkflowActions.WorkflowTransitionActionConfig", b =>
                {
                    b.HasBaseType("WorkflowEngine.Workflow.Model.WorkflowActions.WorkflowActionBaseConfig");

                    b.Property<string>("DestinationState");

                    b.ToTable("WorkflowTransitionActionConfig");

                    b.HasDiscriminator().HasValue("WorkflowTransitionActionConfig");
                });

            modelBuilder.Entity("WorkflowEngine.Workflow.Model.WorkflowListeners.WorkflowWebAPIListenerConfig", b =>
                {
                    b.HasBaseType("WorkflowEngine.Workflow.Model.WorkflowListeners.WorkflowListenerConfigBase");


                    b.ToTable("WorkflowWebAPIListenerConfig");

                    b.HasDiscriminator().HasValue("WorkflowWebAPIListenerConfig");
                });

            modelBuilder.Entity("WorkflowEngine.Workflow.Engine.WorkflowActions.ServerConfig", b =>
                {
                    b.HasOne("WorkflowEngine.Workflow.Model.WorkflowActions.WorkflowQueueActionConfig")
                        .WithMany("Servers")
                        .HasForeignKey("WorkflowQueueActionConfigId");
                });

            modelBuilder.Entity("WorkflowEngine.Workflow.Model.Parameters.WorkflowParameter", b =>
                {
                    b.HasOne("WorkflowEngine.Workflow.Model.WorkflowActions.WorkflowActionBaseConfig")
                        .WithMany("InboundParameters")
                        .HasForeignKey("WorkflowActionBaseConfigId");

                    b.HasOne("WorkflowEngine.Workflow.Model.WorkflowActions.WorkflowActionBaseConfig")
                        .WithMany("ReturnValues")
                        .HasForeignKey("WorkflowActionBaseConfigId1");

                    b.HasOne("WorkflowEngine.Workflow.Model.WorkflowStates.WorkflowStateConfig")
                        .WithMany("WorkflowParameters")
                        .HasForeignKey("WorkflowStateConfigId");
                });

            modelBuilder.Entity("WorkflowEngine.Workflow.Model.Rules.WorkflowRuleConfig", b =>
                {
                    b.HasOne("WorkflowEngine.Workflow.Model.Parameters.WorkflowParameter", "LeftHand")
                        .WithMany()
                        .HasForeignKey("LeftHandId");

                    b.HasOne("WorkflowEngine.Workflow.Model.Parameters.WorkflowParameter", "RightHand")
                        .WithMany()
                        .HasForeignKey("RightHandId");

                    b.HasOne("WorkflowEngine.Workflow.Model.WorkflowStates.WorkflowStateConfig")
                        .WithMany("WorkflowExecutionRules")
                        .HasForeignKey("WorkflowStateConfigId");
                });

            modelBuilder.Entity("WorkflowEngine.Workflow.Model.WorkflowActions.WorkflowActionBaseConfig", b =>
                {
                    b.HasOne("WorkflowEngine.Workflow.Model.WorkflowActions.WorkflowRoutine", "WorkflowRoutine")
                        .WithMany()
                        .HasForeignKey("WorkflowRoutineId");
                });

            modelBuilder.Entity("WorkflowEngine.Workflow.Model.WorkflowEvents.ConditionalOption", b =>
                {
                    b.HasOne("WorkflowEngine.Workflow.Model.WorkflowEvents.WorkflowConditionConfig")
                        .WithMany("ConditionalActions")
                        .HasForeignKey("WorkflowConditionConfigId");
                });

            modelBuilder.Entity("WorkflowEngine.Workflow.Model.WorkflowEvents.WorkflowConditionConfig", b =>
                {
                    b.HasOne("WorkflowEngine.Workflow.Model.Parameters.WorkflowParameter", "ConditionalParamter")
                        .WithMany()
                        .HasForeignKey("ConditionalParamterId");
                });

            modelBuilder.Entity("WorkflowEngine.Workflow.Model.WorkflowListeners.ListenerActionMap", b =>
                {
                    b.HasOne("WorkflowEngine.Workflow.Model.WorkflowListeners.WorkflowListenerConfigBase")
                        .WithMany("ListenerActions")
                        .HasForeignKey("WorkflowListenerConfigBaseId");
                });

            modelBuilder.Entity("WorkflowEngine.Workflow.Model.WorkflowObjects.ObjectField", b =>
                {
                    b.HasOne("WorkflowEngine.Workflow.Model.WorkflowObjects.WorkflowObjectType")
                        .WithMany("ObjectFields")
                        .HasForeignKey("WorkflowObjectTypeId");
                });

            modelBuilder.Entity("WorkflowEngine.Workflow.Model.WorkflowActions.WorkflowConditionalActionConfig", b =>
                {
                    b.HasOne("WorkflowEngine.Workflow.Model.WorkflowEvents.WorkflowConditionConfig", "WorkflowConditions")
                        .WithMany()
                        .HasForeignKey("WorkflowConditionsId");
                });

            modelBuilder.Entity("WorkflowEngine.Workflow.Model.WorkflowActions.WorkflowMutateActionConfig", b =>
                {
                    b.HasOne("WorkflowEngine.Workflow.Model.Parameters.WorkflowParameter", "MutationParameter")
                        .WithMany()
                        .HasForeignKey("MutationParameterId");
                });
        }
    }
}
