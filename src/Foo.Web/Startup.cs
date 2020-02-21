using Foo.Web.Boots;
using Foo.Web._Demos;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace Foo.Web
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMyCookiePolicy();
            services.AddMyMvc();

            services.AddMyServiceLocator();
            services.AddMyLogging(true);

            services.AddFoo();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseMyErrorHandling(env);
            app.UseCookiePolicy();
            app.UseMyStaticFiles();
            app.UseMyMvc();

            app.UseMyServiceLocator();
            app.UseMyLogging();
        }
    }
}
