using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Foo.Web.Api
{
    [Route("Api/Foo")]
    public class FooApi : BaseFooApi
    {
        private readonly ILogger<FooApi> _logger;

        public FooApi(ILogger<FooApi> logger)
        {
            _logger = logger;
        }

        [HttpGet("GetFoo")]
        public string GetFoo()
        {
            var msg = "Log FROM " + this.GetType().Name + " " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            _logger.LogInformation(">> " + msg);
            return msg;
        }

        [HttpGet("GetEx")]
        public string GetEx()
        {
            var msg = "GetEx FROM " + this.GetType().Name + " " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            _logger.LogInformation(">> " + msg);
            var applicationEx = new ApplicationException("shit happens!");
            throw applicationEx;
        }
    }
}
