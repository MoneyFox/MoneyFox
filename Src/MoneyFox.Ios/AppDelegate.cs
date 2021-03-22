using Autofac;
using Foundation;
using Microsoft.Identity.Client;
using MoneyFox.Application;
using MoneyFox.Application.Common.Constants;
using NLog;
using NLog.Config;
using NLog.Targets;
using System;
using System.Globalization;
using System.IO;
using UIKit;
using UserNotifications;
using Xamarin.Essentials;
using Logger = NLog.Logger;
using LogLevel = NLog.LogLevel;

#nullable enable

namespace MoneyFox.iOS
{
    // The UIApplicationDelegate for the application. This class is responsible for launching the
    // User Interface of the application, as well as listening (and optionally responding) to
    // application events from iOS.
    [Register("AppDelegate")]
    public class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
    {
        private const int NOTIFICATION_NEW_MAJOR_VERSION = 10;
        private const int NOTIFICATION_LEGACY_MAJOR_VERSION = 8;

        private Logger? logManager;

        //
        // This method is invoked when the application has loaded and is ready to run. In this
        // method you should instantiate the window, load the UI into it and then make the window
        // visible.
        //
        // You have 17 seconds to return from this method, or iOS will terminate your application.
        //
        public override bool FinishedLaunching(UIApplication uiApplication, NSDictionary launchOptions)
        {
            InitLogger();
            RegisterServices();

            Rg.Plugins.Popup.Popup.Init();

            Xamarin.Forms.Forms.SetFlags("CollectionView_Experimental");
            Xamarin.Forms.Forms.Init();
            Xamarin.Forms.FormsMaterial.Init();

            LoadApplication(new App());

            UINavigationBar.Appearance.Translucent = false;

            RequestToastPermissions(uiApplication);

            return base.FinishedLaunching(uiApplication, launchOptions);
        }

        // Needed for auth
        public override bool OpenUrl(UIApplication app, NSUrl url, NSDictionary options)
        {
            AuthenticationContinuationHelper.SetAuthenticationContinuationEventArgs(url);
            return true;
        }

        private void RequestToastPermissions(UIApplication app)
        {
            // Request Permissions
            if(UIDevice.CurrentDevice.CheckSystemVersion(NOTIFICATION_NEW_MAJOR_VERSION, 0))
            {
                // Request Permissions
                UNUserNotificationCenter.Current.RequestAuthorization(UNAuthorizationOptions.Alert | UNAuthorizationOptions.Badge | UNAuthorizationOptions.Sound, (granted, error) =>
                {
                    // Do something if needed
                });
            }
            else if(UIDevice.CurrentDevice.CheckSystemVersion(NOTIFICATION_LEGACY_MAJOR_VERSION, 0))
            {
                var notificationSettings = UIUserNotificationSettings.GetSettingsForTypes(
                UIUserNotificationType.Alert | UIUserNotificationType.Badge | UIUserNotificationType.Sound, null);

                app.RegisterUserNotificationSettings(notificationSettings);
            }
        }

        private void RegisterServices()
        {
            logManager?.Debug("Register Services.");

            var builder = new ContainerBuilder();
            builder.RegisterModule<IosModule>();
            ViewModelLocator.RegisterServices(builder);

            logManager?.Debug("Register Services finished.");
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
