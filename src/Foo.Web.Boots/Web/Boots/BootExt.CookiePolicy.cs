using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace Foo.Web.Boots
{
    public static partial class BootExt
    {
        public static void AddMyCookiePolicy(this IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });
        }

        public static void UseMyCookiePolicy(this IApplicationBuilder app)
        {
            app.UseCookiePolicy();
        }
    }
}
