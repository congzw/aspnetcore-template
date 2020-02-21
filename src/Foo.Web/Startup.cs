using Foo.Web.Boots;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace Foo.Web
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddBasic();
            services.AddMyServiceLocator();

            services.AddFoo();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseBasic(env);
            app.UseMyServiceLocator();
        }
    }
}
