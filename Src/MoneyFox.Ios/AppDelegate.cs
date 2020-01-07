using Autofac;
using CommonServiceLocator;
using Foundation;
using MediatR;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
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
using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using UIKit;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using XF.Material.iOS;
using Logger = NLog.Logger;
using LogLevel = NLog.LogLevel;

#if !DEBUG
using Microsoft.AppCenter;
using PCLAppConfig;
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

        /// <inheritdoc/>
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

            Forms.Init();
            FormsMaterial.Init();
            Material.Init();
            LoadApplication(new App());
            Popup.Init();

            UIApplication.SharedApplication.StatusBarStyle = UIStatusBarStyle.BlackOpaque;
            uiApplication.SetMinimumBackgroundFetchInterval(MINIMUM_BACKGROUND_FETCH_INTERVAL);
            UIApplication.SharedApplication.SetMinimumBackgroundFetchInterval(UIApplication.BackgroundFetchIntervalMinimum);

            RunAppStartAsync().FireAndForgetSafeAsync();

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
                Analytics.TrackEvent("Start background fetch.");

                await SyncBackupAsync();
                await ClearPaymentsAsync();
                await CreateRecurringPaymentsAsync();

                successful = true;
                logManager.Debug("Background fetch finished successfully");
            }
            catch(Exception ex)
            {
                successful = false;
                Debug.Write(ex);
                Crashes.TrackError(ex);
            }

            completionHandler(successful
                              ? UIBackgroundFetchResult.NewData
                              : UIBackgroundFetchResult.Failed);
        }

        public override async void WillEnterForeground(UIApplication uiApplication)
        {
            base.WillEnterForeground(uiApplication);

            await SyncBackupAsync();
            await ClearPaymentsAsync();
            await CreateRecurringPaymentsAsync();
        }

        private async Task SyncBackupAsync()
        {
            var settingsFacade = new SettingsFacade(new SettingsAdapter());

            if(!settingsFacade.IsBackupAutouploadEnabled || !settingsFacade.IsLoggedInToBackupService)
                return;

            try
            {
                var backupService = ServiceLocator.Current.GetInstance<IBackupService>();
                await backupService.RestoreBackupAsync();
            }
            catch(Exception ex)
            {
                logManager.Error(ex, "Sync Backup Failed.");
                Debug.Write(ex);
            }
            finally
            {
                settingsFacade.LastExecutionTimeStampSyncBackup = DateTime.Now;
            }
        }

        private async Task ClearPaymentsAsync()
        {
            var settingsFacade = new SettingsFacade(new SettingsAdapter());
            try
            {
                logManager.Debug("ClearPayment started.");

                var mediator = ServiceLocator.Current.GetInstance<IMediator>();
                await mediator.Send(new ClearPaymentsCommand());

                logManager.Debug("ClearPayments Job Finished.");
            }
            catch(Exception ex)
            {
                logManager.Error(ex, "Clear Payments Failed!");
            }
            finally
            {
                settingsFacade.LastExecutionTimeStampClearPayments = DateTime.Now;
            }
        }

        private async Task CreateRecurringPaymentsAsync()
        {
            var settingsFacade = new SettingsFacade(new SettingsAdapter());

            try
            {
                logManager.Debug("RecurringPayment Job started.");

                var mediator = ServiceLocator.Current.GetInstance<IMediator>();
                await mediator.Send(new CreateRecurringPaymentsCommand());

                logManager.Debug("RecurringPayment Job finished.");

            }
            catch (Exception ex)
            {
                logManager.Error(ex, "RecurringPayment Job Failed!");
            }
            finally
            {
                settingsFacade.LastExecutionTimeStampRecurringPayments = DateTime.Now;
            }
        }
    }
}
