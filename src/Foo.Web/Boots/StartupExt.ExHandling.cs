using Microsoft.AspNetCore.Builder;

namespace Foo.Web.Boots
{
    public partial class StartupExt
    {
        private static void UseMyErrorHandling(IApplicationBuilder app)
        {
            //if (env.IsDevelopment())
            //{
            //    app.UseDeveloperExceptionPage();
            //}
            //else
            //{
            //    app.UseExceptionHandler("api/error");
            //}
            app.UseExceptionHandler("/Api/MyError");
        }
    }
}
