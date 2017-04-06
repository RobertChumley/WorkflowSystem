using System;
using System.Threading.Tasks;
using FormsModel;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using WorkflowEngine.Workflow.Engine.WorkflowContainer;
using WorkflowEngine.Workflow.Engine.WorkflowRegistries;
using WorkflowEngine.Workflow.Listeners;
using WorkflowEngine.Workflow.Support;

namespace WorkflowNetAPI
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true);

            if (env.IsEnvironment("Development"))
            {
                // This will push telemetry data through Application Insights pipeline faster, allowing you to view results immediately.
                builder.AddApplicationInsightsSettings(developerMode: true);
            }

            builder.AddEnvironmentVariables();
            Configuration = builder.Build();

            ListenerRegistry = RegistryContainer.Resolve<WorkflowListenerRegistry>;
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<FormsContext>(options =>
            options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
            // Add framework services.
            services.AddApplicationInsightsTelemetry(Configuration);
            services.AddMvc();
            services.Configure<FormOptions>(options =>
            {
                options.KeyLengthLimit = 100000;
            });
            services.AddCors(options =>
            {
                
                options.AddPolicy("CorsPolicy", builder =>
                {
                    builder.AllowAnyHeader()
                         .AllowAnyMethod()
                         .AllowAnyOrigin()
                         .AllowCredentials();
                });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            app.UseApplicationInsightsRequestTelemetry();
            
            app.UseApplicationInsightsExceptionTelemetry();
            app.UseCors("CorsPolicy");
            app.UseRouter(new RouteBuilder(app)
                .MapRoute("objroute/{objType}", BuildRouteMap)
                .MapRoute("objroute/{objType}/{id}", BuildRouteMap)
                .Build());
            app.UseMvc();
        }
        
        private Func<WorkflowListenerRegistry> ListenerRegistry { get; }
         
        private  Task BuildRouteMap(HttpContext context)
        {
            return GetListener(context.RouteObject()).OnMessage(context, ContextExtensions.BuildWorkflowObj);
        }

        private WebApiListener GetListener(string routeObject)
        {
            return ((WebApiListener) ListenerRegistry()[routeObject + "API"]);
        }
    }
}
