using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using WorkflowEngine.Workflow.Listeners;
using WorkflowEngine.Workflow.Model;
using WorkflowEngine.Workflow.Support;

namespace WorkflowEngine.Test
{
    [TestClass]
    public class WorkflowListenerTests
    {
        [TestMethod]
        public void TestMethod1()
        {
        }
        [TestMethod]
        public void ListenerWorksTest()
        {
            var config = JsonConvert.DeserializeObject<WorkflowConfiguration>(File.ReadAllText(@"./TestCases/test.json"), new WorkflowActionConfigConverter());
            var definition = new WorkflowController().IntializeWorkflow(null)(config);
            definition.RegisterListeners();
        }
    }
}
