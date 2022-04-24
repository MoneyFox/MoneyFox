namespace MoneyFox.Droid
{

    using Android.App;
    using Android.Content;
    using Android.Content.PM;
    using Android.OS;
    using Android.Runtime;
    using Infrastructure.DbBackup;
    using Microsoft.Identity.Client;
    using Rg.Plugins.Popup;
    using Xamarin.Forms;
    using Xamarin.Forms.Platform.Android;
    using Platform = Xamarin.Essentials.Platform;

    [Activity(
        Label = "MoneyFox",
        Theme = "@style/MainTheme",
        ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.UiMode | ConfigChanges.Orientation)]
    public class MainActivity : FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            ParentActivityWrapper.ParentActivity = this;
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;
            SetStatusBarColor(Color.Black.ToAndroid());
            base.OnCreate(savedInstanceState);
            Popup.Init(this);
            Platform.Init(activity: this, bundle: savedInstanceState);
            Forms.Init(activity: this, bundle: savedInstanceState);
            FormsMaterial.Init(context: this, bundle: savedInstanceState);
            LoadApplication(new App());
        }

        // Needed for auth, so that MSAL can intercept the response from the browser
        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode: requestCode, resultCode: resultCode, data: data);
            AuthenticationContinuationHelper.SetAuthenticationContinuationEventArgs(requestCode: requestCode, resultCode: resultCode, data: data);
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Permission[] grantResults)
        {
            Platform.OnRequestPermissionsResult(requestCode: requestCode, permissions: permissions, grantResults: grantResults);
            base.OnRequestPermissionsResult(requestCode: requestCode, permissions: permissions, grantResults: grantResults);
        }

        public override void OnBackPressed()
        {
            Popup.SendBackPressed(base.OnBackPressed);
        }
    }

}
