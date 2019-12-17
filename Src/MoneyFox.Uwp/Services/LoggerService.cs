using System.IO;
using Windows.Storage;
using NLog;
using NLog.Config;
using NLog.Targets;

#if !DEBUG
using PCLAppConfig;
#endif

namespace MoneyFox.Uwp.Services
{
    public static class LoggerService
    {
        public static void Initialize()
        {
            var config = new LoggingConfiguration();

            // Configure file
            var logfile = new FileTarget("logfile")
            {
                FileName = Path.Combine(ApplicationData.Current.LocalCacheFolder.Path, "moneyfox.log"),
                AutoFlush = true,
                ArchiveEvery = FileArchivePeriod.Month
            };
            
            // Configure console
            var debugTarget = new DebugTarget("console");

#if !DEBUG
            // Configure AppCenter
            var appCenterTarget = new AppCenterTarget("appcenter")
            {
                AppSecret = ConfigurationManager.AppSettings["WindowsAppcenterSecret"]
            };

            config.AddRule(LogLevel.Debug, LogLevel.Fatal, appCenterTarget);
#endif


            config.AddRule(LogLevel.Info, LogLevel.Fatal, debugTarget);
            config.AddRule(LogLevel.Debug, LogLevel.Fatal, logfile);


            LogManager.Configuration = config;
        }
    }
}
