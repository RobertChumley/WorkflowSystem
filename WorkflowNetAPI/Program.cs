using System.IO;
using Microsoft.AspNetCore.Hosting;
using Newtonsoft.Json;
using WorkflowEngine;
using WorkflowEngine.Workflow.Model;
using WorkflowEngine.Workflow.Support;

namespace WorkflowNetAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var config = JsonConvert.DeserializeObject<WorkflowConfiguration>(File.ReadAllText(@"./Configs/test.json"), new WorkflowActionConfigConverter());
            var definition = new WorkflowController().IntializeWorkflow(null)(config);
            
           
            var host = new WebHostBuilder()
                .UseKestrel()
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseIISIntegration()
                .UseStartup<Startup>()
                .Build();

            host.Run();
        }
    }
}
