namespace MoneyFox.Ui;

using Common;
using Core.Common.Interfaces;
using Core.Interfaces;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Identity.Client;
using Microsoft.Maui;
using Microsoft.Maui.ApplicationModel;
using MoneyFox.Infrastructure.DbBackup.Legacy;
using MoneyFox.Ui.Platforms.Android.Resources.Src;

[Activity(Theme = "@style/Maui.SplashTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density)]
public class MainActivity : MauiAppCompatActivity
{
    private const string MSAL_APPLICATIONID = "00a3e4cd-b4b0-4730-be62-5fcf90a94a1d";

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Blocker Bug", "S2857:SQL keywords should be delimited by whitespace", Justification = "Is not SQL")]
    private const string MSAL_URI = $"msal{MSAL_APPLICATIONID}://auth";

    protected override void OnCreate(Bundle? savedInstanceState)
    {
        App.AddPlatformServicesAction = AddServices;
        ParentActivityWrapper.ParentActivity = this;
        base.OnCreate(savedInstanceState);
        Platform.Init(activity: this, bundle: savedInstanceState);
    }

    private static void AddServices(IServiceCollection services)
    {
        services.AddSingleton<IDbPathProvider, DbPathProvider>();
        services.AddSingleton<IStoreOperations, PlayStoreOperations>();
        services.AddSingleton<IAppInformation, DroidAppInformation>();
        services.AddTransient<IFileStore>(_ => new FileStoreIoBase(Application.Context.FilesDir?.Path ?? ""));
        RegisterIdentityClient(services);
    }
    private static void RegisterIdentityClient(IServiceCollection serviceCollection)
    {
        IPublicClientApplication publicClientApplication = PublicClientApplicationBuilder
            .Create(MSAL_APPLICATIONID)
            .WithRedirectUri(MSAL_URI)
            .Build();

        _ = serviceCollection.AddSingleton(publicClientApplication);
    }

    protected override void OnActivityResult(int requestCode, Result resultCode, Intent? data)
    {
        base.OnActivityResult(requestCode: requestCode, resultCode: resultCode, data: data);
        AuthenticationContinuationHelper.SetAuthenticationContinuationEventArgs(requestCode: requestCode, resultCode: resultCode, data: data);
    }

    public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Permission[] grantResults)
    {
        Platform.OnRequestPermissionsResult(requestCode: requestCode, permissions: permissions, grantResults: grantResults);
        base.OnRequestPermissionsResult(requestCode: requestCode, permissions: permissions, grantResults: grantResults);
    }
}
