using Android.App;
using Android.Content;
using Cirrious.MvvmCross.Droid.Platform;
using Cirrious.MvvmCross.ViewModels;
using MoneyManager.Core;
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