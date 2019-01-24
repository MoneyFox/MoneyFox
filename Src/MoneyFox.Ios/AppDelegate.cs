using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using EntityFramework.DbContextScope;
using Foundation;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using MoneyFox.Business.Adapter;
using MoneyFox.Business.Manager;
using MoneyFox.Business.Services;
using MoneyFox.DataAccess;
using MoneyFox.DataAccess.DataServices;
using MoneyFox.Foundation.Constants;
using MoneyFox.Foundation.Interfaces;
using MoneyFox.iOS.Authentication;
using MvvmCross;
using MvvmCross.Forms.Platforms.Ios.Core;
using MvvmCross.Plugin.File;
using PCLAppConfig;
using Rg.Plugins.Popup;
using UIKit;
using Xamarin.Forms.Platform.iOS;

#if !DEBUG
using Microsoft.AppCenter;
#endif

namespace MoneyFox.iOS
{
    // The UIApplicationDelegate for the application. This class is responsible for launching the 
    // User Interface of the application, as well as listening (and optionally responding) to 
    // application events from iOS.
    [Register("AppDelegate")]
    public class AppDelegate : MvxFormsApplicationDelegate<Setup, CoreApp, App>
    {
        // Minimum number of seconds between a background refresh
        // 15 minutes = 60 * 60 = 3600 seconds
        private const double MINIMUM_BACKGROUND_FETCH_INTERVAL = 3600;

        /// <inheritdoc />
        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            ConfigurationManager.Initialise(PCLAppConfig.FileSystemStream.PortableStream.Current);
#if !DEBUG
            AppCenter.Start("3893339f-4e2d-40a9-b415-46ce59c23a8f", typeof(Analytics), typeof(Crashes));
#endif

            UINavigationBar.Appearance.BarTintColor = StyleHelper.PrimaryColor.ToUIColor();
            UINavigationBar.Appearance.TintColor = UIColor.White;

            UIApplication.SharedApplication.StatusBarStyle = UIStatusBarStyle.BlackOpaque;
            app.SetMinimumBackgroundFetchInterval(MINIMUM_BACKGROUND_FETCH_INTERVAL);
            UIApplication.SharedApplication.SetMinimumBackgroundFetchInterval(UIApplication.BackgroundFetchIntervalMinimum);

            ApplicationContext.DbPath = GetLocalFilePath();
            SQLitePCL.Batteries.Init();
            Popup.Init();

            return base.FinishedLaunching(app, options);
        }

        private string GetLocalFilePath()
        {
            string docFolder = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            string libFolder = Path.Combine(docFolder, "..", "Library", "Databases");

            if (!Directory.Exists(libFolder))
            {
                Directory.CreateDirectory(libFolder);
            }

            return Path.Combine(libFolder, DatabaseConstants.DB_NAME);
        }

        public override async void PerformFetch(UIApplication application, Action<UIBackgroundFetchResult> completionHandler)
        {
            if(!Mvx.IoCProvider.CanResolve<IRecurringPaymentManager>() || !Mvx.IoCProvider.CanResolve<IBackupManager>()) return;

            Debug.Write("Enter Background Task");
            var successful = false;
            try
            {
                Analytics.TrackEvent("Start background fetch.");

                await SyncBackup();
                await ClearPayments();
                await CreateRecurringPayments();

                successful = true;
                Analytics.TrackEvent("Background fetch finished successfully.");
            }
            catch (Exception ex)
            {
                Debug.Write(ex);
                Crashes.TrackError(ex);
            }

            completionHandler(successful ? UIBackgroundFetchResult.NewData : UIBackgroundFetchResult.Failed);
        }

        public override async void WillEnterForeground(UIApplication uiApplication)
        {
            base.WillEnterForeground(uiApplication);

            await SyncBackup();
            await ClearPayments();
            await CreateRecurringPayments();
        }

        private async Task SyncBackup()
        {
            var settingsManager = new SettingsManager(new SettingsAdapter());

            try
            {
                ApplicationContext.DbPath = GetLocalFilePath();

                await new BackupManager(new OneDriveService(new OneDriveAuthenticator()),
                                        Mvx.IoCProvider.Resolve<IMvxFileStore>(),
                                        settingsManager,
                                        new ConnectivityAdapter())
                    .DownloadBackup();
            } 
            catch (Exception ex)
            {
                Debug.Write(ex);
            } 
            finally
            {
                settingsManager.LastExecutionTimeStampSyncBackup = DateTime.Now;
            }
        }

        private async Task ClearPayments()
        {
            var settingsManager = new SettingsManager(new SettingsAdapter());
            try
            {
                Debug.WriteLine("ClearPayments Job started");
                ApplicationContext.DbPath = GetLocalFilePath();

                var paymentService = new PaymentService(new AmbientDbContextLocator(), new DbContextScopeFactory());

                var payments = await paymentService.GetUnclearedPayments(DateTime.Now);
                var unclearedPayments = payments.ToList();

                if (unclearedPayments.Any())
                {
                    Debug.WriteLine("Payments for clearing found.");
                    await paymentService.SavePayments(unclearedPayments.ToArray());
                }

                Debug.WriteLine("ClearPayments Job finished.");
            } 
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            } 
            finally
            {
                settingsManager.LastExecutionTimeStampClearPayments = DateTime.Now;
            }
        }

        private async Task CreateRecurringPayments()
        {
            var settingsManager = new SettingsManager(new SettingsAdapter());

            try
            {
                Debug.WriteLine("RecurringPayment Job started.");
                ApplicationContext.DbPath = GetLocalFilePath();

                var ambientDbContextLocator = new AmbientDbContextLocator();
                var dbContextScopeFactory = new DbContextScopeFactory();

                await new RecurringPaymentManager(
                        new RecurringPaymentService(ambientDbContextLocator, dbContextScopeFactory),
                        new PaymentService(ambientDbContextLocator, dbContextScopeFactory))
                    .CreatePaymentsUpToRecur();

                Debug.WriteLine("RecurringPayment Job finished.");
            } 
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            } 
            finally
            {
                settingsManager.LastExecutionTimeStampRecurringPayments = DateTime.Now;
            }
        }
    }
}
