using Android.App;
using Android.Content.PM;
using HockeyApp;
using HockeyApp.Metrics;
using MoneyFox.Shared.Constants;
using MvvmCross.Droid.Views;

namespace MoneyFox.Droid.Activities
{
    [Activity(Label = "Money Fox", MainLauncher = true, Icon = "@mipmap/ic_launcher", Theme = "@style/AppTheme.Splash",
        NoHistory = true, ScreenOrientation = ScreenOrientation.Portrait)]
    public class SplashScreenActivity : MvxSplashScreenActivity
    {
        public SplashScreenActivity() : base(Resource.Layout.activity_splash_screen)
        {
#if !DEBUG
            CrashManager.Register(this, ServiceConstants.HOCKEY_APP_DROID_ID);
            MetricsManager.Register(this, Application, ServiceConstants.HOCKEY_APP_DROID_ID);
#endif
        }
    }
}