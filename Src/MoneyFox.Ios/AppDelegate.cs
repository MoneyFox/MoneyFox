using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using Autofac;
using CommonServiceLocator;
using Foundation;
using MediatR;
using Microsoft.Identity.Client;
using MoneyFox.Application.Common;
using MoneyFox.Application.Common.Adapters;
using MoneyFox.Application.Common.CloudBackup;
using MoneyFox.Application.Common.Constants;
using MoneyFox.Application.Common.Facades;
using MoneyFox.Application.Payments.Commands.ClearPayments;
using MoneyFox.Application.Payments.Commands.CreateRecurringPayments;
using MoneyFox.Presentation;
using MoneyFox.Presentation.Utilities;
using NLog;
using NLog.Config;
using NLog.Targets;
using PCLAppConfig;
using PCLAppConfig.FileSystemStream;
using Rg.Plugins.Popup;
using UIKit;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using XF.Material.iOS;
using Logger = NLog.Logger;
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

        private readonly Logger logManager = LogManager.GetCurrentClassLogger();

        /// <inheritdoc />
        public override bool FinishedLaunching(UIApplication uiApplication,
                                               NSDictionary launchOptions)
        {
            ExecutingPlatform.Current = AppPlatform.iOS;
            ConfigurationManager.Initialise(PortableStream.Current);
            InitLogger();

#if !DEBUG
            AppCenter.Start(ConfigurationManager.AppSettings["IosAppcenterSecret"], typeof(Analytics), typeof(Crashes));
#endif
            RegisterServices();
            RunAppStartAsync().FireAndForgetSafeAsync();

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

        protected async Task RunAppStartAsync()
        {
            await SyncBackupAsync();
            await ClearPaymentsAsync();
            await CreateRecurringPaymentsAsync();
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

        public override async void PerformFetch(UIApplication application,
                                                Action<UIBackgroundFetchResult> completionHandler)
        {
            logManager.Debug("Background fetch started.");
            var successful = false;
            try
            {
                await SyncBackupAsync();

                successful = true;
                logManager.Debug("Background fetch finished successfully");
            }
            catch (Exception ex)
            {
                successful = false;
                logManager.Warn(ex, "Background fetch finished unsuccessfully!");
            }

            completionHandler(successful
                                  ? UIBackgroundFetchResult.NewData
                                  : UIBackgroundFetchResult.Failed);
        }

        public override async void WillEnterForeground(UIApplication uiApplication)
        {
            base.WillEnterForeground(uiApplication);

            await SyncBackupAsync();
        }

        private async Task SyncBackupAsync()
        {
            var settingsFacade = new SettingsFacade(new SettingsAdapter());

            if (!settingsFacade.IsBackupAutouploadEnabled || !settingsFacade.IsLoggedInToBackupService)
                return;

            try
            {
                var backupService = ServiceLocator.Current.GetInstance<IBackupService>();
                await backupService.RestoreBackupAsync();

                var mediator = ServiceLocator.Current.GetInstance<IMediator>();
                await mediator.Send(new ClearPaymentsCommand());
                await mediator.Send(new CreateRecurringPaymentsCommand());
            }
            catch (Exception ex)
            {
                logManager.Error(ex, "Sync Backup Failed.");
                Debug.Write(ex);
            }
            finally
            {
                settingsFacade.LastExecutionTimeStampSyncBackup = DateTime.Now;
            }
        }
    }
}
