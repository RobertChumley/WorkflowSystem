using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Newtonsoft.Json.Linq;
using WorkflowEngine.Workflow.Listeners;

namespace WorkflowEngine.Workflow.Support
{
    public static class ContextExtensions
    {
        public static WorkflowDataObject BuildWorkflowObj(HttpContext context)
        {
            return new RouteObjectBuilder().Initialize()(QueryParamsFunc()(context.Request), context.RouteObject(), context.RoutePrimaryIdentifier());
        }
        public static Func<HttpRequest, Dictionary<string, object>> QueryParamsFunc()
        {
            return (request) => request.ContentLength== null ? request.QueryDictionary() : request.GetJsonBody().ToPropDictionary();
        }
        public static Task WhenMethod(this HttpContext @this, string method,Func<HttpContext, Task> del)
        {
            return @this.Request.Method == method ? del(@this) : null;   
        }
        public static string RoutePrimaryIdentifier(this HttpContext @this)
        {
            return @this.GetRouteValue("id") == null ? "" : @this.GetRouteValue("id").ToString();
        }
        public static string RouteObject(this HttpContext @this)
        {
            return @this.GetRouteValue("objType").ToString();
        }

        public static JObject GetJsonBody(this HttpRequest request)
        {
            return JObject.Parse(request.Body.GetStringBody());
        }
        public static JObject ToJsonObject(this Stream istream)
        {
            return JObject.Parse(istream.GetStringBody());
        }
        public static string GetStringBody(this Stream istream)
        {
            return new StreamReader(istream).DisposeStatement((stream) => ((StreamReader)stream).ReadToEnd());
        }
        public static Dictionary<string, object> QueryDictionary(this HttpRequest request)
        {
            return request.Query.ToDictionary(i => i.Key, i => (object) i.Value);
        }
        public static Dictionary<string, object> ToPropDictionary(this JObject jObject)
        {
            return jObject.Properties().ToDictionary(i => i.Name, i => (object)i.Value);
        }
    }
}