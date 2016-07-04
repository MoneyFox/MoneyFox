using Android.App;
using Android.Content.PM;
using MvvmCross.Droid.Views;

namespace MoneyFox.Droid.Activities
{
    [Activity(Label = "Money Fox",
        MainLauncher = true,
        Icon = "@drawable/ic_launcher",
        Theme = "@style/AppTheme.Splash",
        NoHistory = true,
        ScreenOrientation = ScreenOrientation.Portrait)]
    public class SplashScreenActivity : MvxSplashScreenActivity
    {
        public SplashScreenActivity() : base(Resource.Layout.activity_splash_screen)
        {
        }
    }
}