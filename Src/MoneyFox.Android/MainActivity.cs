namespace MoneyFox.Droid
{
    using global::Android.App;
    using global::Android.Content;
    using global::Android.Content.PM;
    using global::Android.OS;
    using global::Android.Runtime;

    [Activity(
        Label = "MoneyFox",
        Theme = "@style/MainTheme",
        ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.UiMode | ConfigChanges.Orientation)]
    public class MainActivity : MauiAppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
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
    }

}
