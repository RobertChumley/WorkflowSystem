using System;
using System.IO;
using System.Linq;
using KafkaNet;
using KafkaNet.Common;
using KafkaNet.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using WorkflowEngine.Workflow.Model;
using WorkflowEngine.Workflow.Model.WorkflowActions;
using WorkflowEngine.Workflow.Support;


namespace WorkflowEngine.Test
{
    [TestClass]
    public class LoadWorkflowConfigTests
    {
        [TestMethod]
        public void LoadConfigTest()
        {
            var config = JsonConvert.DeserializeObject<WorkflowConfiguration>(File.ReadAllText(@"./TestCases/test.json"),new WorkflowActionConfigConverter() );
            Assert.AreNotSame(0, config.WorkflowActions.Count);
            Assert.AreNotSame(0, config.WorkflowStates.Count);
            Assert.AreNotSame(0, config.WorkflowTransitions.Count);
            Assert.AreNotSame(0, config.WorkflowEvents.Count);

            var timerActionConfig = (WorkflowTimerActionConfig)config.WorkflowActions[0];
            Assert.AreNotSame(0, timerActionConfig.Tick);
        }
        [TestMethod]
        public void InitializeWorkflowTest()
        {
            var config = JsonConvert.DeserializeObject<WorkflowConfiguration>(File.ReadAllText(@"./TestCases/test.json"), new WorkflowActionConfigConverter());
            var definition = new WorkflowController().IntializeWorkflow(null);
            Assert.AreEqual("Initial",definition(config)
                .CurrentActionManager()
                .StateManager()
                .CurrentWorkflowState
                .WorkflowStateConfiguration
                .StateName);
        }
        [TestMethod]
        public void WorkflowTransitionTest()
        {
            var config = JsonConvert.DeserializeObject<WorkflowConfiguration>(File.ReadAllText(@"./TestCases/test.json"), new WorkflowActionConfigConverter());
            var definition = new WorkflowController().IntializeWorkflow(null);
            
            Assert.AreEqual("State 2", definition(config)
                .ExecuteCustomEvent((actionRegistry) => actionRegistry["Action 3"])()
                .CurrentActionManager()
                .StateManager()
                .CurrentWorkflowState
                .WorkflowStateConfiguration
                .StateName);
        }
        [TestMethod]
        public void WorkflowMutationTest()
        {
            var config = JsonConvert.DeserializeObject<WorkflowConfiguration>(File.ReadAllText(@"./TestCases/test.json"), new WorkflowActionConfigConverter());
            var definition = new WorkflowController().IntializeWorkflow(null)(config);

            Assert.AreEqual("State 2",
                definition.ExecuteCustomEvent((actionRegistry) => actionRegistry["Action 3"])()
                .CurrentActionManager()
                .StateManager()
                .CurrentWorkflowState
                .WorkflowStateConfiguration
                .StateName);

            Assert.AreEqual("2", definition
                .ExecuteCustomEvent((actionRegistry) => actionRegistry["Action 4"])()
                .CurrentActionManager()
                .StateManager()
                .CurrentWorkflowState
                .CurrentParameterValues["Name1"]
                .Value);
        }
        [TestMethod]
        public void WorkflowRuleExecutionTest()
        {
            var config = JsonConvert.DeserializeObject<WorkflowConfiguration>(File.ReadAllText(@"./TestCases/test.json"), new WorkflowActionConfigConverter());
            var definition = new WorkflowController().IntializeWorkflow(null)(config);

            Assert.AreEqual("State 4", definition
                .ExecuteCustomEvent((actionRegistry) => actionRegistry["Action 3"])()
                .ExecuteCustomEvent((actionRegistry) => actionRegistry["Action 8"])()
                .CurrentActionManager()
                .StateManager()
                .CurrentWorkflowState
                .WorkflowStateConfiguration
                .StateName);

            Assert.AreEqual("State 3", definition
                .ExecuteCustomEvent((actionRegistry) => actionRegistry["Action 3"])()
                .ExecuteCustomEvent((actionRegistry) => actionRegistry["Action 4"])()
                .ExecuteCustomEvent((actionRegistry) => actionRegistry["Action 8"])()
                .CurrentActionManager()
                .StateManager()
                .CurrentWorkflowState
                .WorkflowStateConfiguration
                .StateName);


        }
        [TestMethod]
        public void WorkflowConditionalExecutionTest()
        {
            //ToDo: Conditional execution is not correct, it should be able to derive the conditional parameter from anywhere
            var config = JsonConvert.DeserializeObject<WorkflowConfiguration>(File.ReadAllText(@"./TestCases/test.json"), new WorkflowActionConfigConverter());
            var definition = new WorkflowController().IntializeWorkflow(null)(config);
            Assert.AreEqual("State 4", definition
                .ExecuteCustomEvent((actionRegistry) => actionRegistry["Action 9"])()
                .CurrentActionManager()
                .StateManager()
                .CurrentWorkflowState
                .WorkflowStateConfiguration
                .StateName);
        }
        [TestMethod]
        public void WorkflowTimerExecutionTest()
        {
            var config = JsonConvert.DeserializeObject<WorkflowConfiguration>(File.ReadAllText(@"./TestCases/test.json"), new WorkflowActionConfigConverter());
            var definition = new WorkflowController().IntializeWorkflow(null)(config);
            Assert.AreNotEqual("1", definition
                .ExecuteCustomEvent((actionRegistry) => actionRegistry["Action 3"])()
                .ExecuteCustomEvent((actionRegistry) => actionRegistry["Action 10"])()
                .CurrentActionManager()
                .StateManager()
                .CurrentWorkflowState
                .CurrentParameterValues["Name1"]
                .Value);
        }

        [TestMethod]
        public void WorkflowActionChainingExecutionTest()
        {
            var config = JsonConvert.DeserializeObject<WorkflowConfiguration>(File.ReadAllText(@"./TestCases/test.json"), new WorkflowActionConfigConverter());
            var definition = new WorkflowController().IntializeWorkflow(null)(config);
            Assert.AreEqual("State 3", definition
               .ExecuteCustomEvent((actionRegistry) => actionRegistry["Action 12"])()
               .CurrentActionManager()
               .StateManager()
               .CurrentWorkflowState
               .WorkflowStateConfiguration
               .StateName);

        }
        [TestMethod]
        public void WorkflowQueueExecutionTest()
        {
            var config = JsonConvert.DeserializeObject<WorkflowConfiguration>(File.ReadAllText(@"./TestCases/test.json"), new WorkflowActionConfigConverter());
            var definition = new WorkflowController().IntializeWorkflow(null)(config);

            definition.ExecuteCustomEvent((actionRegistry) => actionRegistry["Action 11"])();

            var options = new KafkaOptions(new Uri("http://192.168.99.100:9092"));
            var router = new BrokerRouter(options);
            var consumer = new Consumer(new ConsumerOptions("workflowtest", router));

            var amessage = consumer.Consume().FirstOrDefault();

            Assert.IsNotNull(amessage);
            Assert.AreEqual("Action 6", amessage.Value.ToUtf8String());

        }
        [TestMethod]
        public void BuildSQLTablesTest()
        {
            var config = JsonConvert.DeserializeObject<WorkflowConfiguration>(File.ReadAllText(@"./TestCases/test.json"), new WorkflowActionConfigConverter());
            var definition = new WorkflowController().IntializeWorkflow(null)(config);
            definition.BuildSqlTables();
        }
        [TestMethod]
        public void SocketSendTest()
        {
            var config = JsonConvert.DeserializeObject<WorkflowConfiguration>(File.ReadAllText(@"./TestCases/test.json"), new WorkflowActionConfigConverter());
            var definition = new WorkflowController().IntializeWorkflow(null)(config);
            definition.ExecuteCustomEvent((actionRegistry) => actionRegistry["Action 13"])();
        }

    }
}
