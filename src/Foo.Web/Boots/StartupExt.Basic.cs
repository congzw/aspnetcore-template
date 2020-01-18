using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace Foo.Web.Boots
{
    public static partial class StartupExt
    {
        public static void AddBasic(this IServiceCollection services)
        {

            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            var mvcBuilder = services.AddMvc(opts =>
            {
                //Here it is being added globally. 
                //Could be used as attribute on selected controllers instead
                opts.Filters.Add(new CustomJSONExceptionFilter());
            });
            mvcBuilder.SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }

        public static void UseBasic(this IApplicationBuilder app, IHostingEnvironment env)
        {
            UseMyErrorHandling(app,env);
            UseMyStaticFiles(app);

            app.UseCookiePolicy();
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
