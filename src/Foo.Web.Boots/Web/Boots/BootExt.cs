using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Foo.Web.Boots
{
    public static partial class BootExt
    {
        public static IServiceCollection AddAllBootExt(this IServiceCollection services)
        {
            services.AddMyServiceLocator();
            return services;
        }

        public static void UseAllBootExt(this IApplicationBuilder app)
        {
            app.UseMyServiceLocator();
        }
    }
}
