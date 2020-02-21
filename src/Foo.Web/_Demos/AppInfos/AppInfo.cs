using System.Net.Http.Headers;

namespace Foo.Web._Demos.AppInfos
{
    public class AppInfo
    {
        public AppInfo()
        {
            AppName = "FooApp";
            Version = "1.0.0";
        }

        public string AppName { get; set; }
        public string Version { get; set; }
    }

    public interface IAppInfoService
    {
        AppInfo GetAppInfo();
    }

    public class AppInfoService : IAppInfoService
    {
        public AppInfo GetAppInfo()
        {
            //todo more   
            return new AppInfo();
        }
    }
}
