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
            services.AddMvcCore();
        }

        public static void UseBasic(this IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            UseFooStaticFiles(app);

            app.UseMvc();
            
            app.Run(async (context) =>
            {
                await context.Response.WriteAsync("Hello World!");
            });
        }
    }
}
