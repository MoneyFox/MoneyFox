using System;
using System.IO;
using Foundation;
using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using MoneyFox.DataAccess;
using MoneyFox.Foundation.Constants;
using MvvmCross.Forms.Platforms.Ios.Core;
using Rg.Plugins.Popup;
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
#if !DEBUG
            AppCenter.Start("3893339f-4e2d-40a9-b415-46ce59c23a8f", typeof(Analytics), typeof(Crashes));
#endif

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
    }
}
