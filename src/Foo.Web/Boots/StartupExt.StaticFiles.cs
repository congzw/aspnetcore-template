using System.Collections.Generic;
using System.IO;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.FileProviders;

namespace Foo.Web.Boots
{
    public static partial class StartupExt
    {
        private static void UseFooStaticFiles(IApplicationBuilder app)
        {
            app.UseDefaultFiles(new DefaultFilesOptions() { DefaultFileNames = new List<string>() { "index.html" } });

            app.UseStaticFiles(new StaticFileOptions
            {
                ContentTypeProvider = new FileExtensionContentTypeProvider
                {
                    Mappings = { [".vue"] = "text/html" }
                }
            });

            //make both rq ok:
            //rq => MyImages/oops.png
            //rq => images/oops.png

            var fileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images"));
            var imageRequestPath = "/MyImages";
            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = fileProvider,
                RequestPath = imageRequestPath
            });

            //make directory browser ok:
            app.UseDirectoryBrowser(new DirectoryBrowserOptions
            {
                FileProvider = fileProvider,
                RequestPath = imageRequestPath
            });
        }
    }
}
