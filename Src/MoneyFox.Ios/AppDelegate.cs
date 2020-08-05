using Autofac;
using Foundation;
using Microsoft.Identity.Client;
using MoneyFox.Common;
using NLog;
using NLog.Config;
using NLog.Targets;
using System.IO;
using UIKit;
using Xamarin.Essentials;
using Logger = NLog.Logger;
using LogLevel = NLog.LogLevel;

namespace MoneyFox.iOS
{
    // The UIApplicationDelegate for the application. This class is responsible for launching the
    // User Interface of the application, as well as listening (and optionally responding) to
    // application events from iOS.
    [Register("AppDelegate")]
    public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
    {
        private Logger logManager;

        //
        // This method is invoked when the application has loaded and is ready to run. In this
        // method you should instantiate the window, load the UI into it and then make the window
        // visible.
        //
        // You have 17 seconds to return from this method, or iOS will terminate your application.
        //
        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            InitLogger();
            RegisterServices();

            Rg.Plugins.Popup.Popup.Init();

            global::Xamarin.Forms.Forms.SetFlags("CollectionView_Experimental");
            global::Xamarin.Forms.Forms.Init();
            global::Xamarin.Forms.FormsMaterial.Init();

            LoadApplication(new App());

            return base.FinishedLaunching(app, options);
        }

        // Needed for auth
        public override bool OpenUrl(UIApplication app, NSUrl url, NSDictionary options)
        {
            AuthenticationContinuationHelper.SetAuthenticationContinuationEventArgs(url);
            return true;
        }

        private void RegisterServices()
        {
            logManager.Debug("Register Services.");

            var builder = new ContainerBuilder();
            builder.RegisterModule<IosModule>();
            ViewModelLocator.RegisterServices(builder);

            logManager.Debug("Register Services finished.");
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

            config.AddRule(LogLevel.Info, LogLevel.Fatal, debugTarget);
            config.AddRule(LogLevel.Debug, LogLevel.Fatal, logfile);

            LogManager.Configuration = config;
            logManager = LogManager.GetCurrentClassLogger();
        }
    }
}
