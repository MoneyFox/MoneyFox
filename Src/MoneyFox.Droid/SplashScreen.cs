namespace MoneyFox.Droid
{
    [Activity(Label = "Money Fox", MainLauncher = true, Icon = "@drawable/icon", Theme = "@style/AppTheme.Splash",
        NoHistory = true, ScreenOrientation = ScreenOrientation.Portrait)]
    public class SplashScreen : MvxSplashScreenActivity
    {
        public SplashScreen() : base(Resource.Layout.splash_screen)
        {
        }
    }
}