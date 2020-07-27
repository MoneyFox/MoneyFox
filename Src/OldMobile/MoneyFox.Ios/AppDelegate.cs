using Autofac;
using Foundation;
using Microsoft.Identity.Client;
using MoneyFox.Application.Common;
using MoneyFox.Application.Common.Constants;
using MoneyFox.Presentation;
using NLog;
using NLog.Config;
using NLog.Targets;
using PCLAppConfig;
using PCLAppConfig.FileSystemStream;
using Rg.Plugins.Popup;
using System.IO;
using UIKit;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using XF.Material.iOS;
using LogLevel = NLog.LogLevel;

#if !DEBUG
using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
#endif

namespace MoneyFox.iOS
{
    [Register(nameof(AppDelegate))]
    public class AppDelegate : FormsApplicationDelegate
    {
        // Minimum number of seconds between a background refresh
        // 15 minutes = 60 * 60 = 3600 seconds
        private const double MINIMUM_BACKGROUND_FETCH_INTERVAL = 3600;

        /// <inheritdoc/>
        public override bool FinishedLaunching(UIApplication uiApplication, NSDictionary launchOptions)
        {
            ExecutingPlatform.Current = AppPlatform.iOS;
            ConfigurationManager.Initialise(PortableStream.Current);
            InitLogger();

            RegisterServices();

#if !DEBUG
            AppCenter.Start(ConfigurationManager.AppSettings["IosAppcenterSecret"], typeof(Analytics), typeof(Crashes));
#endif

            Forms.Init();
            FormsMaterial.Init();
            Material.Init();
            LoadApplication(new App());
            Popup.Init();

            UIApplication.SharedApplication.StatusBarStyle = UIStatusBarStyle.BlackOpaque;
            uiApplication.SetMinimumBackgroundFetchInterval(MINIMUM_BACKGROUND_FETCH_INTERVAL);
            UIApplication.SharedApplication.SetMinimumBackgroundFetchInterval(UIApplication.BackgroundFetchIntervalMinimum);

            return base.FinishedLaunching(uiApplication, launchOptions);
        }

        private static void RegisterServices()
        {
            var builder = new ContainerBuilder();
            builder.RegisterModule<IosModule>();
            ViewModelLocator.RegisterServices(builder);
        }

        private void InitLogger()
        {
            var config = new LoggingConfiguration();

            var logfile = new FileTarget("logfile")
                          {
                              FileName = Path.Combine(FileSystem.CacheDirectory, AppConstants.LogFileName),
                              AutoFlush = true,
                              ArchiveEvery = FileArchivePeriod.Month
                          };
            var debugTarget = new DebugTarget("console");

#if !DEBUG
            // Configure AppCenter
            var appCenterTarget = new AppCenterTarget("appcenter")
            {
                AppSecret = ConfigurationManager.AppSettings["IosAppcenterSecret"]
            };

            config.AddRule(LogLevel.Debug, LogLevel.Fatal, appCenterTarget);
#endif

            config.AddRule(LogLevel.Info, LogLevel.Fatal, debugTarget);
            config.AddRule(LogLevel.Debug, LogLevel.Fatal, logfile);

            LogManager.Configuration = config;
        }

        // Needed for auth
        public override bool OpenUrl(UIApplication app, NSUrl url, NSDictionary options)
        {
            AuthenticationContinuationHelper.SetAuthenticationContinuationEventArgs(url);
            return true;
        }
    }
}
