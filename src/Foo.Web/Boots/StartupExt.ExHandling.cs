using System;
using System.Net;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Foo.Web.Boots
{
    public partial class StartupExt
    {
        private static void UseMyErrorHandling(IApplicationBuilder app, IHostingEnvironment env)
        {
            //all return html
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }
        }
    }

    public class MyJSONExceptionFilter : ExceptionFilterAttribute
    {
        public override void OnException(ExceptionContext context)
        {
            var serviceProvider = context.HttpContext.RequestServices;
            var ex = context.Exception;

            //记录异常
            var logger = serviceProvider.GetService<ILogger<MyJSONExceptionFilter>>();
            logger.LogError(ex, ex.Message);

            //对ajax json 按约定处理
            if ("application/json".Equals(context.HttpContext.Request.ContentType, StringComparison.OrdinalIgnoreCase))
            {
                var messageResult = new MessageResult() { Message = ex.Message, Data = ex, Success = false };
                var jsonResult = new JsonResult(messageResult);
                jsonResult.StatusCode = (int)HttpStatusCode.InternalServerError;
                context.Result = jsonResult;
                return;
            }

            //对普通请求，按不同阶段的逻辑显示
            var hostEnvironment = serviceProvider.GetService<IHostingEnvironment>();
            var isDev = hostEnvironment.IsDevelopment();
            if (isDev)
            {
                //不处理，则默认配置有机会处理，例如： app.UseDeveloperExceptionPage();
                return;
            }

            //非开发调试阶段
            var viewResult = new ContentResult();
            viewResult.ContentType = "text/html; charset=utf-8"; //解决中文乱码的问题
            viewResult.Content = string.Format(
@"<html>
    <head>
        <title>异常信息</title>
    </head>
    <body>
        <h2>Sorry, 发生了一个异常</h2>
        <p>{0}</p>
        <hr/>
        <p>{1}</p>
    </body>
</html>",
            ex.Message,
            "详情请查看日志记录");
            context.Result = viewResult;
        }

        public class MessageResult
        {
            public bool Success { get; set; }
            public string Message { get; set; }
            public object Data { get; set; }
        }
    }
}
