using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace Foo.Web.Boots
{
    public static partial class StartupExt
    {
        public static void AddBasic(this IServiceCollection services)
        {
            services.AddMvcCore()
                .AddJsonFormatters() //fix: StatusCode 406 (Not Acceptable) in ASP.NET Core
                ; 
        }

        public static void UseBasic(this IApplicationBuilder app, IHostingEnvironment env)
        {
            UseMyErrorHandling(app);
            UseMyStaticFiles(app);

            app.UseMvc();

            app.Run(async (context) =>
            {
                await context.Response.WriteAsync("Hello World!");
            });
        }
    }
}
