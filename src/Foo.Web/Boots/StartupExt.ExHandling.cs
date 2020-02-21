using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;

namespace Foo.Web.Boots
{
    public partial class StartupExt
    {
        internal static void UseMyErrorHandling(this IApplicationBuilder app, IHostingEnvironment env)
        {
            //return html for pages
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            //return json for apis
            app.UseMyExceptionMiddleware();
        }
    }

}
