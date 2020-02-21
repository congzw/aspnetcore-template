using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace Foo.Web.Boots
{
    public static partial class BootExt
    {
        public static void AddMyMvc(this IServiceCollection services)
        {
            var mvcBuilder = services.AddMvc(opts =>
            {
                //Here it is being added globally. Could be used as attribute on selected controllers instead
                //opts.Filters.Add(new MyJSONExceptionFilter());
            });
            mvcBuilder.SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }

        public static void UseMyMvc(this IApplicationBuilder app)
        {
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
