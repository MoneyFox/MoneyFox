namespace MoneyFox.Droid
{

    using AndroidX.AppCompat.App;
    using global::Android.App;
    using global::Android.Content.PM;

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
