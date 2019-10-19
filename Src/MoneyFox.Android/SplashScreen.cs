using Android.App;
using Android.Content.PM;
using Android.Support.V7.App;

namespace MoneyFox.Droid
{
    [Activity(
        Label = "MoneyFox",
        MainLauncher = true,
        Icon = "@drawable/ic_launcher",
        Theme = "@style/Theme.Splash",
        NoHistory = true,
        ScreenOrientation = ScreenOrientation.Portrait)]
    public class SplashScreen : AppCompatActivity
    {
        protected override void OnResume()
        {
            base.OnResume();
            StartActivity(typeof(MainActivity));
        }
    }
}
