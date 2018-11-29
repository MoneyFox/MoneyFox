using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using MoneyFox.Business.ViewModels;
using MvvmCross;
using MvvmCross.Forms.Platforms.Android.Views;

namespace MoneyFox.Droid
{
	 [Activity(
		 Label = "MoneyFox"
		 , MainLauncher = true
		 , Icon = "@drawable/ic_launcher"
		 , Theme = "@style/Theme.Splash"
		 , NoHistory = true
		 , ScreenOrientation = ScreenOrientation.Portrait,
		  Name = "com.applysolutions.moneyfox.SplashScreen")]
	 [MetaData("android.app.shortcuts", Resource = "@xml/shortcuts")]
	 public class SplashScreen : MvxFormsSplashScreenActivity<Setup, CoreApp, App>
	 {
		  public SplashScreen()
			  : base(Resource.Layout.SplashScreen)
		  {
		  }

		  protected override Task RunAppStartAsync(Bundle bundle)
		  {
			   Intent intent = new Intent(this, typeof(MainActivity));
			   intent.SetAction(Intent.Action);
			   StartActivity(intent);
			   return Task.CompletedTask;
		  }
	 }
}