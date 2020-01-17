using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Foo.Web.Api
{
    [Route("Api/Test")]
    public class TestApi : BaseFooApi
    {
        private readonly ILogger<TestApi> _testApiLog;

        public TestApi(ILogger<TestApi> testApiLog)
        {
            _testApiLog = testApiLog;
        }

        [HttpGet("GetLog")]
        public string GetLog()
        {
            //Fatal   Something bad happened; application is going down
            //Error Something failed; application may or may not continue
            //Warn Something unexpected; application will continue
            //Info Normal behavior like mail sent, user updated profile etc.
            //Debug For debugging; executed query, user authenticated, session expired
            //Trace For trace debugging; begin method X, end method X

            var msg = "Log FROM " + this.GetType().Name + " " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            _testApiLog.LogInformation(">> " + msg);
            return msg;
        }
    }
}
