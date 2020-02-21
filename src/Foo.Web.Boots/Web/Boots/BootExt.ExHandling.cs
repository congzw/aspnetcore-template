using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Foo.Web.Boots
{
    public partial class StartupExt
    {
        public static void UseMyErrorHandling(this IApplicationBuilder app, IHostingEnvironment env)
        {
            //return html for pages
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            //return json for apis
            app.UseMyExceptionMiddleware();
        }
    }

    public static class MyExceptionMiddlewareExtensions
    {
        ////how to use
        ////return html for pages
        //if (env.IsDevelopment())
        //{
        //    app.UseDeveloperExceptionPage();
        //}
        //else
        //{
        //    app.UseExceptionHandler("/Home/Error");
        //}

        //app.UseMyExceptionMiddleware(MyExceptionHandleJsonOptions.Default, MyExceptionHandleHtmlOptions.Default);

        public static IApplicationBuilder UseMyExceptionMiddleware(this IApplicationBuilder builder,
            MyExceptionHandleJsonOptions jsonOptions = MyExceptionHandleJsonOptions.Default,
            MyExceptionHandleHtmlOptions htmlOptions = MyExceptionHandleHtmlOptions.Default)
        {
            MyExceptionMiddleware.JsonOptions = jsonOptions;
            MyExceptionMiddleware.HtmlOptions = htmlOptions;
            return builder.UseMiddleware<MyExceptionMiddleware>();
        }
    }
    public class MyExceptionMiddleware
    {
        public static MyExceptionHandleJsonOptions JsonOptions { get; set; }
        public static MyExceptionHandleHtmlOptions HtmlOptions { get; set; }

        private readonly RequestDelegate _next;
        private readonly ILogger<MyExceptionMiddleware> _logger;

        public MyExceptionMiddleware(RequestDelegate next, ILoggerFactory loggerFactory)
        {
            _next = next ?? throw new ArgumentNullException(nameof(next));
            _logger = loggerFactory?.CreateLogger<MyExceptionMiddleware>() ?? throw new ArgumentNullException(nameof(loggerFactory));
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                var serviceProvider = context.RequestServices;
                var hostEnvironment = serviceProvider.GetService<IHostingEnvironment>();
                var isDev = hostEnvironment.IsDevelopment();

                var isJson = "application/json".Equals(context.Request.ContentType, StringComparison.OrdinalIgnoreCase);

                if (isJson)
                {
                    var shouldJsonThrow = OptionsCheck.ShouldJsonThrow(JsonOptions, isDev);
                    if (shouldJsonThrow)
                    {
                        throw;
                    }
                    await InvokeJson(context, ex);
                }
                else
                {
                    var shouldHtmlThrow = OptionsCheck.ShouldHtmlThrow(HtmlOptions, isDev);
                    if (shouldHtmlThrow)
                    {
                        throw;
                    }
                    await InvokeHtml(context, ex);
                }
            }
        }

        private async Task InvokeJson(HttpContext context, Exception ex)
        {
            context.Response.Clear();
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            var messageResult = new MyExceptionResult() { Message = ex.Message, Data = ex, Success = false };
            var result = new JsonResult(messageResult);
            var routeData = context.GetRouteData();
            var actionDescriptor = new ActionDescriptor();
            var actionContext = new ActionContext(context, routeData, actionDescriptor);
            await result.ExecuteResultAsync(actionContext);
        }

        private async Task InvokeHtml(HttpContext context, Exception ex)
        {
            var htmlContent = CreateHtml(ex);
            var result = new ContentResult();
            result.Content = htmlContent;
            result.StatusCode = (int)HttpStatusCode.InternalServerError;
            result.ContentType = "text/html; charset=utf-8";

            var routeData = context.GetRouteData();
            var actionDescriptor = new ActionDescriptor();
            var actionContext = new ActionContext(context, routeData, actionDescriptor);
            await result.ExecuteResultAsync(actionContext);
        }

        private string CreateHtml(Exception ex)
        {
            var htmlContent = string.Format(
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
            return htmlContent;

        }
    }
    public class MyExceptionResult
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public object Data { get; set; }
    }
    public class OptionsCheck
    {
        public static bool ShouldJsonThrow(MyExceptionHandleJsonOptions options, bool isDev)
        {
            if (!options.HasFlag(MyExceptionHandleJsonOptions.Dev) && isDev)
            {
                return true;
            }
            if (!options.HasFlag(MyExceptionHandleJsonOptions.Pro) && !isDev)
            {
                return true;
            }
            return false;
        }
        public static bool ShouldHtmlThrow(MyExceptionHandleHtmlOptions options, bool isDev)
        {
            if (!options.HasFlag(MyExceptionHandleHtmlOptions.Dev) && isDev)
            {
                return true;
            }
            if (!options.HasFlag(MyExceptionHandleHtmlOptions.Pro) && !isDev)
            {
                return true;
            }
            return false;
        }
    }
    [Flags]
    public enum MyExceptionHandleJsonOptions
    {
        None = 0,
        Dev = 1 << 0,
        Pro = 1 << 1,
        Default = Dev | Pro,
        All = Dev | Pro
    }
    [Flags]
    public enum MyExceptionHandleHtmlOptions
    {
        None = 0,
        Dev = 1 << 0,
        Pro = 1 << 1,
        Default = Pro,
        All = Dev | Pro
    }
}
