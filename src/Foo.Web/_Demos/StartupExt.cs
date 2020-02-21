using Foo.Web._Demos.AppInfos;
using Microsoft.Extensions.DependencyInjection;

namespace Foo.Web._Demos
{
    public static partial class StartupExt
    {
        public static void AddFoo(this IServiceCollection services)
        {
            services.AddTransient<IAppInfoService, AppInfoService>();
        }
    }
}
