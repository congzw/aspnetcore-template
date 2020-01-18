using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;

namespace Foo.Web.Boots
{
    public partial class StartupExt
    {
        private static void UseMyErrorHandling(IApplicationBuilder app, IHostingEnvironment env)
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
