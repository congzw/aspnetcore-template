using System;
using System.Diagnostics;
using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Foo.Web.Boots
{
    public partial class StartupExt
    {
        private static void UseMyErrorHandling(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                DeveloperExceptionPageOptions options = new DeveloperExceptionPageOptions();
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("api/error");
            }
            //app.UseExceptionHandler("/Api/MyError");
        }
    }

    public class CustomJSONExceptionFilter : ExceptionFilterAttribute
    {
        public override void OnException(ExceptionContext context)
        {
            if ("application/json".Equals(context.HttpContext.Request.ContentType, StringComparison.OrdinalIgnoreCase))
            {
                var jsonResult = new JsonResult(new { error = context.Exception.Message });
                jsonResult.StatusCode = (int)System.Net.HttpStatusCode.InternalServerError;
                context.Result = jsonResult;
            }
        }
    }
}
