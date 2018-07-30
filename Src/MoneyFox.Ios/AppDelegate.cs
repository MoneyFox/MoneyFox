using System;
using System.IO;
using Foundation;
using MoneyFox.DataAccess;
using MoneyFox.Foundation.Constants;
using MvvmCross.Forms.Platforms.Ios.Core;
using UIKit;

namespace MoneyFox.iOS
{
    // The UIApplicationDelegate for the application. This class is responsible for launching the 
    // User Interface of the application, as well as listening (and optionally responding) to 
    // application events from iOS.
    [Register("AppDelegate")]
    public partial class AppDelegate : MvxFormsApplicationDelegate<Setup, CoreApp, App>
    {
        //
        // This method is invoked when the application has loaded and is ready to run. In this 
        // method you should instantiate the window, load the UI into it and then make the window
        // visible.
        //
        // You have 17 seconds to return from this method, or iOS will terminate your application.
        //
        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            ApplicationContext.DbPath = GetLocalFilePath();
            SQLitePCL.Batteries.Init();

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
        [Export("application:performFetchWithCompletionHandler:")]
        public override async void PerformFetch(UIApplication application, Action<UIBackgroundFetchResult> completionHandler)
        {
            Debug.Write("Enter Background Task");
            var successful = false;
            try
            {
                Analytics.TrackEvent("Start background fetch.");

                await ClearPayments();
                await Mvx.Resolve<IRecurringPaymentManager>().CreatePaymentsUpToRecur();
                await Mvx.Resolve<IBackupManager>().DownloadBackup();

                //var tasks = new List<Task>
                //{
                //    ClearPayments(),
                //    Mvx.Resolve<IRecurringPaymentManager>().CreatePaymentsUpToRecur(),
                //    Mvx.Resolve<IBackupManager>().DownloadBackup()
                //};

                //await Task.WhenAll(tasks);

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
            var paymentService = Mvx.Resolve<IPaymentService>();

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
