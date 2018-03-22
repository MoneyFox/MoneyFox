using System;
using System.IO;
using Android.App;
using Android.Content.PM;
using MoneyFox.Foundation.Constants;
using MvvmCross.Droid.Views;

namespace MoneyFox.Droid
{
    [Activity(
        Label = "MoneyFox"
        , MainLauncher = true
        , Icon = "@drawable/ic_launcher"
        , Theme = "@style/Theme.Splash"
        , NoHistory = true
        , ScreenOrientation = ScreenOrientation.Portrait)]
    public class SplashScreen : MvxSplashScreenActivity
    {
        public SplashScreen()
            : base(Resource.Layout.SplashScreen)
        {
            DataAccess.ApplicationContext.DbPath =
                Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal),
                             DatabaseConstants.DB_NAME);
        }

        protected override void TriggerFirstNavigate()
        {
            StartActivity(typeof(MainActivity));
            base.TriggerFirstNavigate();
        }
    }
}