using Foo.Web._Demos.AppInfos;
using Foo.Web._Demos.Lifetimes;
using Microsoft.Extensions.DependencyInjection;

namespace Foo.Web._Demos
{
    public static partial class StartupExt
    {
        public static void AddFoo(this IServiceCollection services)
        {
            services.AddTransient<IAppInfoService, AppInfoService>();
            services.AddSingleton<ISingletonDesc, LifetimeDesc>();
            services.AddScoped<IScopedDesc, LifetimeDesc>();
            services.AddTransient<ITransientDesc, LifetimeDesc>();
        }
    }
}
