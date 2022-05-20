namespace MoneyFox.Droid
{
    using global::Android.App;
    using global::Android.Content;
    using global::Android.Content.PM;
    using global::Android.OS;
    using Microsoft.Identity.Client;
    using Microsoft.Maui;
    using Microsoft.Maui.ApplicationModel;
    using MoneyFox.Infrastructure.DbBackup;

    [Activity(
        Label = "MoneyFox",
        Theme = "@style/MainTheme",
        ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.UiMode | ConfigChanges.Orientation)]
    public class MainActivity : MauiAppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            ParentActivityWrapper.ParentActivity = this;
            base.OnCreate(savedInstanceState);
            Platform.Init(this, savedInstanceState);
        }

        protected override void OnActivityResult(int requestCode, Result resultCode, Intent? data)
        {
            base.OnActivityResult(requestCode, resultCode, data);
            AuthenticationContinuationHelper.SetAuthenticationContinuationEventArgs(requestCode: requestCode, resultCode: resultCode, data: data);
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Permission[] grantResults)
        {
            Platform.OnRequestPermissionsResult(requestCode: requestCode, permissions: permissions, grantResults: grantResults);
            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }


        //// Needed for auth, so that MSAL can intercept the response from the browser
        //protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        //{
        //    base.OnActivityResult(requestCode: requestCode, resultCode: resultCode, data: data);
        //    AuthenticationContinuationHelper.SetAuthenticationContinuationEventArgs(requestCode: requestCode, resultCode: resultCode, data: data);
        //}

        //public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Permission[] grantResults)
        //{
        //    Platform.OnRequestPermissionsResult(requestCode: requestCode, permissions: permissions, grantResults: grantResults);
        //    base.OnRequestPermissionsResult(requestCode: requestCode, permissions: permissions, grantResults: grantResults);
        //}
    }

}
