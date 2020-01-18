using System;
using System.Net;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Foo.Web.Api
{
    [ApiController]
    public class ErrorHandlingApi : ControllerBase
    {
        private readonly ILogger<ErrorHandlingApi> _logger;

        public ErrorHandlingApi(ILogger<ErrorHandlingApi> logger)
        {
            _logger = logger;
        }

        #region normal handling

        [Route("api/error")]
        public ActionResult Error([FromServices] IHostingEnvironment webHostEnvironment)
        {
            var feature = HttpContext.Features.Get<IExceptionHandlerPathFeature>();
            var ex = feature?.Error;
            var isDev = webHostEnvironment.IsDevelopment();
            var problemDetails = new ProblemDetails
            {
                Status = (int)HttpStatusCode.InternalServerError,
                Instance = feature?.Path,
                Title = isDev ? $"{ex.GetType().Name}: {ex.Message}" : "An error occurred.",
                Detail = isDev ? ex.StackTrace : "sorry an error occurred.",
            };

            return StatusCode(problemDetails.Status.Value, problemDetails);
        }

        #endregion

        [Route("Api/MyError")]
        public ActionResult MyError([FromServices] IHostingEnvironment webHostEnvironment)
        {
            var feature = HttpContext.Features.Get<IExceptionHandlerPathFeature>();
            var ex = feature?.Error;
            var isDev = webHostEnvironment.IsDevelopment();
            var problemTitle = isDev ? $"{ex.GetType().Name}: {ex.Message}" : "Sorry，请求发生了异常";
            var problemDetails = new ProblemDetails
            {
                Status = (int)HttpStatusCode.InternalServerError,
                Instance = feature?.Path,
                Title = problemTitle,
                Detail = isDev ? ex.StackTrace : "Sorry，请求发生了异常，详细内容请查看日志"
            };

            if (ex != null)
            {
                _logger.LogError(ex, problemTitle);
            }

            //跟前端约定的异常结果对象
            var messageResult = new MessageResult() { Message = problemTitle, Data = problemDetails, Success = false };
            return StatusCode(problemDetails.Status.Value, messageResult);
        }

        public class MessageResult
        {
            public bool Success { get; set; }
            public string Message { get; set; }
            public object Data { get; set; }
        }
    }
}