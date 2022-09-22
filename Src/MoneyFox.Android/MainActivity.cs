namespace MoneyFox.Droid
{

    using Android.App;
    using Android.Content;
    using Android.Content.PM;
    using Android.OS;
    using Android.Runtime;
    using Common;
    using Core.Common.Interfaces;
    using Core.Interfaces;
    using Infrastructure.DbBackup;
    using Infrastructure.DbBackup.Legacy;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Identity.Client;
    using Xamarin.Forms;
    using Xamarin.Forms.Platform.Android;
    using Application = Android.App.Application;
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
            Platform.Init(activity: this, bundle: savedInstanceState);
            Forms.Init(activity: this, bundle: savedInstanceState);
            FormsMaterial.Init(context: this, bundle: savedInstanceState);
            LoadApplication(new App(AddServices));
        }

        private static void AddServices(IServiceCollection services)
        {
            services.AddSingleton<IDbPathProvider, DbPathProvider>();
            services.AddSingleton<IStoreOperations, PlayStoreOperations>();
            services.AddSingleton<IAppInformation, DroidAppInformation>();
            services.AddTransient<IFileStore>(_ => new FileStoreIoBase(Application.Context.FilesDir?.Path ?? ""));
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
