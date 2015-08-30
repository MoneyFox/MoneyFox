using System;
using System.Diagnostics;
using Android.App;
using Android.Content;
using Cirrious.CrossCore;
using Cirrious.CrossCore.Platform;
using Cirrious.MvvmCross.Droid.Platform;
using Cirrious.MvvmCross.ViewModels;
using MoneyManager.Core;
using MoneyManager.Foundation.OperationContracts;
using SQLite.Net.Interop;
using SQLite.Net.Platform.XamarinAndroid;
using Xamarin;

namespace MoneyManager.Droid
{
    public class Setup : MvxAndroidSetup, IMvxTrace
    {
        public Setup(Context applicationContext)
            : base(applicationContext)
        {
        }

        public void Trace(MvxTraceLevel level, string tag, Func<string> message)
        {
            Debug.WriteLine(tag + ":" + level + ":" + message());
        }

        public void Trace(MvxTraceLevel level, string tag, string message)
        {
            Debug.WriteLine(tag + ":" + level + ":" + message);
        }

        public void Trace(MvxTraceLevel level, string tag, string message, params object[] args)
        {
            try
            {
                Debug.WriteLine(tag + ":" + level + ":" + message, args);
            }
            catch (FormatException)
            {
                Trace(MvxTraceLevel.Error, tag, "Exception during trace of {0} {1} {2}", level, message);
            }
        }

        protected override void InitializeFirstChance()
        {
            base.InitializeFirstChance();

            Mvx.RegisterType<ISQLitePlatform, SQLitePlatformAndroid>();
            Mvx.RegisterType<IDatabasePath, DatabasePath>();
            Mvx.RegisterType<IDialogService, DialogService>();
            Mvx.RegisterType<IAppInformation, AppInformation>();
        }


        protected override IMvxApplication CreateApp()
        {
            var insightKey = "e5c4ac56bb1ca47559bc8d4973d0a8c4d78c7648";

#if DEBUG
            insightKey = Insights.DebugModeKey;
#endif
            if (!Insights.IsInitialized)
            {
                Insights.Initialize(insightKey, Application.Context);
            }

            return new App();
        }
    }
}