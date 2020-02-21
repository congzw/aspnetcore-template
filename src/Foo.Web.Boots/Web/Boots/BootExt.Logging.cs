using System;
using Foo.Common;
using Foo.Common.Logs;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;

namespace Foo.Web.Boots
{
    public static partial class BootExt
    {
        private static bool _useNLog = false;

        public static IServiceCollection AddMyLogging(this IServiceCollection services, bool useNLog)
        {
            _useNLog = useNLog;
            services.AddLogging(config =>
            {
                //a hack for ILogger injection!
                config.Services.AddSingleton<LoggerWrapper>();
                config.Services.AddSingleton<ILogger>(sp => sp.GetService<LoggerWrapper>().Logger);

                //for simple log
                config.Services.AddSingleton<NetCoreLogHelper>();
                config.Services.AddSingleton<ILogHelper>(sp => sp.GetService<NetCoreLogHelper>());

                config.ClearProviders();
                config.AddConsole();
                config.AddDebug();

                if (_useNLog)
                {
                    //for NLog
                    config.AddNLog("nlog.config");
                }
            });
            return services;
        }

        public static void UseMyLogging(this IApplicationBuilder app)
        {
            var logHelper = app.ApplicationServices.GetService<ILogHelper>();
            LogHelper.Resolve = () => logHelper;
            logHelper.Info(">>>> LogHelper.Resolve set completed");

            if (_useNLog)
            {
                var applicationLifetime = app.ApplicationServices.GetService<IApplicationLifetime>();
                var logger = app.ApplicationServices.GetService<ILogger>();
                applicationLifetime.ApplicationStopping.Register(OnShutdownNLog, logger);
            }
        }

        private static void OnShutdownNLog(object state)
        {
            var logger = state as ILogger;
            try
            {
                logger?.LogInformation(">>>> OnShutdownNLog");
                NLog.LogManager.Shutdown();
            }
            catch (Exception e)
            {
                logger?.LogError(e, ">>>> OnShutdownNLog Error");
            }
        }
    }

    #region a hack for ILogger injection!
    
    public class LoggerWrapper
    {
        public ILogger Logger { get; set; }

        public LoggerWrapper(ILogger<LoggerWrapper> logger)
        {
            Logger = logger;
        }
    }

    #endregion

    #region form simple log

    public class NetCoreLogHelper : ILogHelper
    {
        public NetCoreLogHelper(ILogger<LogHelper> logger)
        {
            DefaultLogger = logger;
        }

        public void Log(string message, int level)
        {
            DefaultLogger.Log(level.AsLogLevel(), message);
        }

        public ILogger DefaultLogger { get; set; }
    }

    public static class NetCoreLogExtensions
    {
        public static LogLevel AsLogLevel(this int level)
        {
            //Trace = 0,
            //Debug = 1,
            //Information = 2,
            //Warning = 3,
            //Error = 4,
            //Critical = 5,
            //None = 6

            if (level <= 0)
            {
                return LogLevel.Trace;
            }
            if (level <= 1)
            {
                return LogLevel.Debug;
            }
            if (level <= 2)
            {
                return LogLevel.Information;
            }
            if (level <= 3)
            {
                return LogLevel.Warning;
            }
            if (level <= 4)
            {
                return LogLevel.Error;
            }
            //if (level <= 5)
            //{
            //    return LogLevel.Critical;
            //}
            //return LogLevel.None;
            return LogLevel.Critical;
        }
    }

    #endregion
}
