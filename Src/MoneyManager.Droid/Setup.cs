using System.ComponentModel;
using Android.App;
using Android.Content;
using Cirrious.CrossCore;
using Cirrious.MvvmCross.Droid.Platform;
using Cirrious.MvvmCross.ViewModels;
using MoneyManager.Core;
using MoneyManager.Foundation.OperationContracts;
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
        }

        protected override IMvxApplication CreateApp()
        {
#if DEBUG
            if (!Insights.IsInitialized)
            {
                Insights.Initialize("e5c4ac56bb1ca47559bc8d4973d0a8c4d78c7648", Application.Context);
            }
#endif

            return new App();
        }

        protected override void InitializeLastChance()
        {
            base.InitializeLastChance();
            //Mvx.RegisterSingleton<IAppInformation>(new AppInformation());
        }
    }
}