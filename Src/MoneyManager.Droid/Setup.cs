using System.ComponentModel;
using Android.App;
using Android.Content;
using Cirrious.CrossCore;
using Cirrious.MvvmCross.Droid.Platform;
using Cirrious.MvvmCross.ViewModels;
using MoneyManager.Core;
using MoneyManager.Foundation.OperationContracts;
using SQLite.Net.Interop;
using SQLite.Net.Platform.XamarinAndroid;
using Xamarin;

namespace MoneyManager.Droid
{
    public class Setup : MvxAndroidSetup
    {
        public Setup(Context applicationContext)
            : base(applicationContext)
        {
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
            string insightKey = "e5c4ac56bb1ca47559bc8d4973d0a8c4d78c7648";

#if DEBUG
            insightKey = Insights.DebugModeKey;
#endif
            if (!Insights.IsInitialized)
            {
                Insights.Initialize(insightKey, Application.Context);
            }

            return new App();
        }

        protected override void InitializeLastChance()
        {
            base.InitializeLastChance();
            //Mvx.RegisterSingleton<IAppInformation>(new AppInformation());
        }
    }
}