using System.IO;
using Android.App;
using Android.Content.PM;
using Android.OS;
using MoneyFox.Foundation.Constants;
using MvvmCross.Forms.Platforms.Android.Views;
using Environment = System.Environment;

namespace MoneyFox.Droid
{
    [Activity(
        Label = "MoneyFox"
        , MainLauncher = true
        , Icon = "@drawable/ic_launcher"
        , Theme = "@style/Theme.Splash"
        , NoHistory = true
        , ScreenOrientation = ScreenOrientation.Portrait)]
    public class SplashScreen : MvxFormsSplashScreenActivity<Setup, CoreApp, App>
    {
        public SplashScreen()
            : base(Resource.Layout.SplashScreen)
        {
            DataAccess.ApplicationContext.DbPath =
                Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal),
                             DatabaseConstants.DB_NAME);
        }

        protected override void RunAppStart(Bundle bundle)
        {
            StartActivity(typeof(MainActivity));
            base.RunAppStart(bundle);
        }
    }
}