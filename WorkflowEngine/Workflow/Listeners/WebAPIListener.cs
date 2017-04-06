using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using WorkflowEngine.DatabaseEngine;
using WorkflowEngine.Workflow.Engine.WorkflowContainer;
using WorkflowEngine.Workflow.Engine.WorkflowRegistries;
using WorkflowEngine.Workflow.Model.WorkflowListeners;

namespace WorkflowEngine.Workflow.Listeners
{
    public class WebApiListener  : WorkflowListenerBase
    {
        public WebApiListener(Func<WorkflowListenerConfigBase> listenerBaseConfigFunc) : base(listenerBaseConfigFunc)
        {
            WorkflowWebApiListenerConfig = (WorkflowWebApiListenerConfig)listenerBaseConfigFunc();
            RegistryFunc = RegistryContainer.Resolve<WorkflowActionRegistry>;
            DatabaseEngineFunc = RegistryContainer.Resolve<WorkflowDatabaseEngine>;
            WorkflowActionMap = new Dictionary<string, ListenerActionMap>();
            WorkflowWebApiListenerConfig.ListenerActions.ForEach(item=>WorkflowActionMap[item.ListenerMethod] = item);
        }

        public Dictionary<string, ListenerActionMap> WorkflowActionMap { get; }
        public WorkflowWebApiListenerConfig WorkflowWebApiListenerConfig { get; }
        public Func<WorkflowActionRegistry> RegistryFunc { get; }
        public Func<WorkflowDatabaseEngine> DatabaseEngineFunc { get; }

        public Task OnMessage(HttpContext context,Func<HttpContext,WorkflowDataObject> workflowObject)
        {
            //By default the system will perform CRUD, but if this value if specifically
            //WorkflowWebApiListenerConfig.DisableCRUD;
            //ToDo: create custom action for CRUD Override
            //WorkflowWebApiListenerConfig.CRUDOverrides;  
            return new ContextManager(context, null)
                .When("GET",    (con) => con.Response.WriteAsync(DatabaseEngineFunc().ReadData(workflowObject(con)).ToJsonString()))
                .When("POST",   (con) => InsertData(workflowObject(con), con))
                .When("PUT",    (con) => UpdateData(workflowObject(con), con))
                .When("DELETE", (con) => DeleteData(workflowObject(con), con))
                .Execute;
        }
        public Task InsertData(WorkflowDataObject obj, HttpContext con)
        {
            DatabaseEngineFunc().InsertData(obj);
            return WriteDefault(obj, con);
        }
        public Task UpdateData(WorkflowDataObject obj, HttpContext con)
        {
            DatabaseEngineFunc().InsertData(obj);
            return WriteDefault(obj, con);
        }
        public Task DeleteData(WorkflowDataObject obj, HttpContext con)
        {
            DatabaseEngineFunc().InsertData(obj);
            return WriteDefault(obj, con);
        }
        public Task WriteDefault(WorkflowDataObject obj, HttpContext con)
        {
            return con.Response.WriteAsync("Done for obj " + obj.WorkflowObjectType.ObjectTypeName + " values " + 
                string.Join(",", obj.FieldValues.Keys.Select(k => k + "= " + obj.FieldValues[k].ObjectValue)));
        }
    }

    public class ContextManager
    {
        public HttpContext Context { get; }
        public Task Execute { get; }
        public ContextManager(HttpContext context,Task exec)
        {
            Context = context;
            Execute = exec;
        }

        public ContextManager When(string method, Func<HttpContext, Task> execFunc)
        {
            return Context.Request.Method == method ? new ContextManager(Context, execFunc(Context)) : this;
        }
    }
    
    public static class DataTableExtensions
    {
        public static string ToJsonString(this DataTable table)
        {
            return JsonConvert.SerializeObject((from DataRow dr in table.Rows
                        select table.Columns.Cast<DataColumn>().ToDictionary(col => col.ColumnName, col => dr[col])).ToList());   
        }
    }
    public class WorkflowListenerBase
    {
        protected WorkflowListenerBase(Func<WorkflowListenerConfigBase> listenerBaseConfigFunc)
        {
            WorkflowListenerConfig = listenerBaseConfigFunc();
        }
        public WorkflowListenerConfigBase WorkflowListenerConfig { get; }


    }
}
