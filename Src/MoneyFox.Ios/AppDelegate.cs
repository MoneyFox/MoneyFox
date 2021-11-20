#nullable enable
using Autofac;
using Foundation;
using Microsoft.Identity.Client;
using MoneyFox.Application.Common.Constants;
using NLog;
using NLog.Config;
using NLog.Targets;
using Rg.Plugins.Popup;
using System.IO;
using UIKit;
using UserNotifications;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using Logger = NLog.Logger;
using LogLevel = NLog.LogLevel;

namespace MoneyFox.iOS
{
    [Register("AppDelegate")]
    public class AppDelegate : FormsApplicationDelegate
    {
        private const int NOTIFICATION_NEW_MAJOR_VERSION = 10;
        private const int NOTIFICATION_LEGACY_MAJOR_VERSION = 8;

        private Logger? logManager;

        // You have 17 seconds to return from this method, or iOS will terminate your application.
        //
        public override bool FinishedLaunching(UIApplication uiApplication, NSDictionary launchOptions)
        {
            InitLogger();
            RegisterServices();

            Popup.Init();
            Forms.Init();
            FormsMaterial.Init();
            LoadApplication(new App());

            RemoveNavigationBarBorder();
            RequestToastPermissions(uiApplication);

            return base.FinishedLaunching(uiApplication, launchOptions);
        }

        private static void RemoveNavigationBarBorder()
        {
            UINavigationBar.Appearance.Translucent = false;
            if (UIDevice.CurrentDevice.CheckSystemVersion(13, 0))
            {
                var appearance = new UINavigationBarAppearance()
                {
                    BackgroundColor = UIColor.Clear, ShadowColor = null,
                };

                UINavigationBar.Appearance.StandardAppearance = appearance;
                UINavigationBar.Appearance.ScrollEdgeAppearance = appearance;
                UINavigationBar.Appearance.CompactAppearance = appearance;
            }
            else
            {
                UINavigationBar.Appearance.SetBackgroundImage(new UIImage(), UIBarMetrics.Default);
                UINavigationBar.Appearance.ShadowImage = new UIImage();
            }
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
                UNUserNotificationCenter.Current.RequestAuthorization(
                    UNAuthorizationOptions.Alert | UNAuthorizationOptions.Badge | UNAuthorizationOptions.Sound,
                    (granted, error) =>
                    {
                        // Do something if needed
                    });
            }
            else if(UIDevice.CurrentDevice.CheckSystemVersion(NOTIFICATION_LEGACY_MAJOR_VERSION, 0))
            {
                UIUserNotificationSettings notificationSettings = UIUserNotificationSettings.GetSettingsForTypes(
                    UIUserNotificationType.Alert | UIUserNotificationType.Badge | UIUserNotificationType.Sound,
                    null);

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