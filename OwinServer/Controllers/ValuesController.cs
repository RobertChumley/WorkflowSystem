﻿namespace OwinServer.Controllers
{
    using System.Web.Http;

    public class ValuesController : ApiController
    {
        public IHttpActionResult GetValues()
        {
            return Ok(new[] { "a", "b", "c" });
        }
    }
}
