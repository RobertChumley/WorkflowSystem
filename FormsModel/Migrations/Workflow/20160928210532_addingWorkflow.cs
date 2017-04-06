using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Metadata;

namespace FormsModel.Migrations.Workflow
{
    public partial class addingWorkflow : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "WorkflowRoutine",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    MethodName = table.Column<string>(nullable: true),
                    TypeName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkflowRoutine", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WebApiListeners",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Discriminator = table.Column<string>(nullable: false),
                    ListenerType = table.Column<int>(nullable: false),
                    WorkflowObjectName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WebApiListeners", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WorkflowObjectTypes",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ObjectName = table.Column<string>(nullable: true),
                    PrimaryKeyField = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkflowObjectTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WorkflowStateConfigs",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    StateName = table.Column<string>(nullable: true),
                    WorkflowStateType = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkflowStateConfigs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ListenerActionMap",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ListenerAction = table.Column<string>(nullable: true),
                    ListenerMethod = table.Column<string>(nullable: true),
                    WorkflowListenerConfigBaseId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ListenerActionMap", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ListenerActionMap_WebApiListeners_WorkflowListenerConfigBaseId",
                        column: x => x.WorkflowListenerConfigBaseId,
                        principalTable: "WebApiListeners",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ObjectField",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    FieldDisplay = table.Column<string>(nullable: true),
                    FieldName = table.Column<string>(nullable: true),
                    FieldType = table.Column<string>(nullable: true),
                    WorkflowObjectTypeId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ObjectField", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ObjectField_WorkflowObjectTypes_WorkflowObjectTypeId",
                        column: x => x.WorkflowObjectTypeId,
                        principalTable: "WorkflowObjectTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "WorkflowRuleConfig",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ElseAction = table.Column<string>(nullable: true),
                    LeftHandId = table.Column<int>(nullable: true),
                    LeftHandParam = table.Column<string>(nullable: true),
                    PositiveAction = table.Column<string>(nullable: true),
                    RightHandId = table.Column<int>(nullable: true),
                    WorkflowRuleOperator = table.Column<int>(nullable: false),
                    WorkflowStateConfigId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkflowRuleConfig", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WorkflowRuleConfig_WorkflowStateConfigs_WorkflowStateConfigId",
                        column: x => x.WorkflowStateConfigId,
                        principalTable: "WorkflowStateConfigs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "WorkflowActionConfigs",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Discriminator = table.Column<string>(nullable: false),
                    NextAction = table.Column<string>(nullable: true),
                    WorkflowActionName = table.Column<string>(nullable: true),
                    WorkflowActionType = table.Column<int>(nullable: false),
                    WorkflowRoutineId = table.Column<int>(nullable: true),
                    WorkflowConditionsId = table.Column<int>(nullable: true),
                    EventName = table.Column<string>(nullable: true),
                    WorkflowEventType = table.Column<int>(nullable: true),
                    MutationOperatorType = table.Column<int>(nullable: true),
                    MutationParameterId = table.Column<int>(nullable: true),
                    QueueAction = table.Column<string>(nullable: true),
                    QueueName = table.Column<string>(nullable: true),
                    QueueType = table.Column<int>(nullable: true),
                    End = table.Column<DateTime>(nullable: true),
                    Iterations = table.Column<int>(nullable: true),
                    LastExecuted = table.Column<DateTime>(nullable: true),
                    Start = table.Column<DateTime>(nullable: true),
                    Tick = table.Column<int>(nullable: true),
                    TickType = table.Column<int>(nullable: true),
                    TimerAction = table.Column<string>(nullable: true),
                    DestinationState = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkflowActionConfigs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WorkflowActionConfigs_WorkflowRoutine_WorkflowRoutineId",
                        column: x => x.WorkflowRoutineId,
                        principalTable: "WorkflowRoutine",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ServerConfig",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Url = table.Column<string>(nullable: true),
                    WorkflowQueueActionConfigId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServerConfig", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ServerConfig_WorkflowActionConfigs_WorkflowQueueActionConfigId",
                        column: x => x.WorkflowQueueActionConfigId,
                        principalTable: "WorkflowActionConfigs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "WorkflowParameter",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true),
                    Type = table.Column<string>(nullable: true),
                    WorkflowActionBaseConfigId = table.Column<int>(nullable: true),
                    WorkflowActionBaseConfigId1 = table.Column<int>(nullable: true),
                    WorkflowStateConfigId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkflowParameter", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WorkflowParameter_WorkflowActionConfigs_WorkflowActionBaseConfigId",
                        column: x => x.WorkflowActionBaseConfigId,
                        principalTable: "WorkflowActionConfigs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_WorkflowParameter_WorkflowActionConfigs_WorkflowActionBaseConfigId1",
                        column: x => x.WorkflowActionBaseConfigId1,
                        principalTable: "WorkflowActionConfigs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_WorkflowParameter_WorkflowStateConfigs_WorkflowStateConfigId",
                        column: x => x.WorkflowStateConfigId,
                        principalTable: "WorkflowStateConfigs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "WorkflowConditionConfig",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ConditionalParamterId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkflowConditionConfig", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WorkflowConditionConfig_WorkflowParameter_ConditionalParamterId",
                        column: x => x.ConditionalParamterId,
                        principalTable: "WorkflowParameter",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ConditionalOption",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    WorkflowAction = table.Column<string>(nullable: true),
                    WorkflowConditionConfigId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConditionalOption", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ConditionalOption_WorkflowConditionConfig_WorkflowConditionConfigId",
                        column: x => x.WorkflowConditionConfigId,
                        principalTable: "WorkflowConditionConfig",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ServerConfig_WorkflowQueueActionConfigId",
                table: "ServerConfig",
                column: "WorkflowQueueActionConfigId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkflowParameter_WorkflowActionBaseConfigId",
                table: "WorkflowParameter",
                column: "WorkflowActionBaseConfigId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkflowParameter_WorkflowActionBaseConfigId1",
                table: "WorkflowParameter",
                column: "WorkflowActionBaseConfigId1");

            migrationBuilder.CreateIndex(
                name: "IX_WorkflowParameter_WorkflowStateConfigId",
                table: "WorkflowParameter",
                column: "WorkflowStateConfigId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkflowRuleConfig_LeftHandId",
                table: "WorkflowRuleConfig",
                column: "LeftHandId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkflowRuleConfig_RightHandId",
                table: "WorkflowRuleConfig",
                column: "RightHandId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkflowRuleConfig_WorkflowStateConfigId",
                table: "WorkflowRuleConfig",
                column: "WorkflowStateConfigId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkflowActionConfigs_WorkflowRoutineId",
                table: "WorkflowActionConfigs",
                column: "WorkflowRoutineId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkflowActionConfigs_WorkflowConditionsId",
                table: "WorkflowActionConfigs",
                column: "WorkflowConditionsId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkflowActionConfigs_MutationParameterId",
                table: "WorkflowActionConfigs",
                column: "MutationParameterId");

            migrationBuilder.CreateIndex(
                name: "IX_ConditionalOption_WorkflowConditionConfigId",
                table: "ConditionalOption",
                column: "WorkflowConditionConfigId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkflowConditionConfig_ConditionalParamterId",
                table: "WorkflowConditionConfig",
                column: "ConditionalParamterId");

            migrationBuilder.CreateIndex(
                name: "IX_ListenerActionMap_WorkflowListenerConfigBaseId",
                table: "ListenerActionMap",
                column: "WorkflowListenerConfigBaseId");

            migrationBuilder.CreateIndex(
                name: "IX_ObjectField_WorkflowObjectTypeId",
                table: "ObjectField",
                column: "WorkflowObjectTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_WorkflowRuleConfig_WorkflowParameter_LeftHandId",
                table: "WorkflowRuleConfig",
                column: "LeftHandId",
                principalTable: "WorkflowParameter",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_WorkflowRuleConfig_WorkflowParameter_RightHandId",
                table: "WorkflowRuleConfig",
                column: "RightHandId",
                principalTable: "WorkflowParameter",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_WorkflowActionConfigs_WorkflowParameter_MutationParameterId",
                table: "WorkflowActionConfigs",
                column: "MutationParameterId",
                principalTable: "WorkflowParameter",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_WorkflowActionConfigs_WorkflowConditionConfig_WorkflowConditionsId",
                table: "WorkflowActionConfigs",
                column: "WorkflowConditionsId",
                principalTable: "WorkflowConditionConfig",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WorkflowParameter_WorkflowActionConfigs_WorkflowActionBaseConfigId",
                table: "WorkflowParameter");

            migrationBuilder.DropForeignKey(
                name: "FK_WorkflowParameter_WorkflowActionConfigs_WorkflowActionBaseConfigId1",
                table: "WorkflowParameter");

            migrationBuilder.DropTable(
                name: "ServerConfig");

            migrationBuilder.DropTable(
                name: "WorkflowRuleConfig");

            migrationBuilder.DropTable(
                name: "ConditionalOption");

            migrationBuilder.DropTable(
                name: "ListenerActionMap");

            migrationBuilder.DropTable(
                name: "ObjectField");

            migrationBuilder.DropTable(
                name: "WebApiListeners");

            migrationBuilder.DropTable(
                name: "WorkflowObjectTypes");

            migrationBuilder.DropTable(
                name: "WorkflowActionConfigs");

            migrationBuilder.DropTable(
                name: "WorkflowRoutine");

            migrationBuilder.DropTable(
                name: "WorkflowConditionConfig");

            migrationBuilder.DropTable(
                name: "WorkflowParameter");

            migrationBuilder.DropTable(
                name: "WorkflowStateConfigs");
        }
    }
}
