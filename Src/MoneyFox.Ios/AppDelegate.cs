using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Foundation;
#if !DEBUG
using Microsoft.AppCenter;  
#endif
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using MoneyFox.DataAccess;
using MoneyFox.DataAccess.DataServices;
using MoneyFox.Foundation.Constants;
using MoneyFox.Foundation.Interfaces;
using MvvmCross;
using MvvmCross.Forms.Platforms.Ios.Core;
using Rg.Plugins.Popup;
using UIKit;

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
#if !DEBUG
            AppCenter.Start(ConfigurationManager.AppSettings["IosAppcenterSecret"], typeof(Analytics), typeof(Crashes));
#endif

            app.SetMinimumBackgroundFetchInterval(UIApplication.BackgroundFetchIntervalMinimum);
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

                var tasks = new List<Task>
                {
                    ClearPayments(),
                    Mvx.IoCProvider.Resolve<IRecurringPaymentManager>().CreatePaymentsUpToRecur(),
                    Mvx.IoCProvider.Resolve<IBackupManager>().DownloadBackup()
                };

                await Task.WhenAll(tasks);

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

        private async Task ClearPayments()
        {
            if (!Mvx.IoCProvider.CanResolve<IPaymentService>()) return;

            var paymentService = Mvx.IoCProvider.Resolve<IPaymentService>();

            var payments = await paymentService.GetUnclearedPayments(DateTime.Now);
            var unclearedPayments = payments.ToList();

            if (unclearedPayments.Any())
            {
                Debug.WriteLine("Payments for clearing found.");
                await paymentService.SavePayments(unclearedPayments.ToArray());
            }
        }
    }
}
