using System.Threading.Tasks;
using Android.App;
using Android.Content.PM;
using Android.OS;
using MoneyFox.Presentation;
using MvvmCross.Forms.Platforms.Android.Views;

namespace MoneyFox.Droid
{
    [Activity(
        Label = "MoneyFox", 
        MainLauncher = true, 
        Icon = "@drawable/ic_launcher", 
        Theme = "@style/Theme.Splash", 
        NoHistory = true, 
        ScreenOrientation = ScreenOrientation.Portrait)]
    public class SplashScreen : MvxFormsSplashScreenActivity<ApplicationSetup, CoreApp, App>
    {
        public SplashScreen()
            : base(Resource.Layout.SplashScreen)
        {
        }

        protected override Task RunAppStartAsync(Bundle bundle)
        {
            StartActivity(typeof(MainActivity));
            return Task.CompletedTask;
        }
    }
}