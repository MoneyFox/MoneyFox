using Android.App;
using Android.Content.PM;
using AndroidX.AppCompat.App;

namespace MoneyFox.Droid
{
    [Activity(
        Label = "MoneyFox",
        MainLauncher = true,
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