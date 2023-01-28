namespace MoneyFox.Ui;

using Core.Interfaces;
using Foundation;
using JetBrains.Annotations;
using Microsoft.Identity.Client;
using Platforms.iOS.Src;
using UIKit;
using UserNotifications;

[Register("AppDelegate")]
[UsedImplicitly]
public class AppDelegate : MauiUIApplicationDelegate
{
    private const string MSAL_APPLICATIONID = "00a3e4cd-b4b0-4730-be62-5fcf90a94a1d";
    private const string MSAL_URI = $"msal{MSAL_APPLICATIONID}://auth";

    protected override MauiApp CreateMauiApp()
    {
        App.AddPlatformServicesAction = AddServices;
        RequestToastPermissions();

        return MauiProgram.CreateMauiApp();
    }

    private static void AddServices(IServiceCollection services)
    {
        services.AddSingleton<IDbPathProvider, DbPathProvider>();
        services.AddTransient<IFileStore>(_ => new IosFileStore(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)));
        RegisterIdentityClient(services);
    }

    private static void RegisterIdentityClient(IServiceCollection serviceCollection)
    {
        var publicClientApplication = PublicClientApplicationBuilder.Create(MSAL_APPLICATIONID)
            .WithRedirectUri(MSAL_URI)
            .WithIosKeychainSecurityGroup("com.microsoft.adalcache")
            .Build();

        _ = serviceCollection.AddSingleton(publicClientApplication);
    }

    // Needed for auth
    public override bool OpenUrl(UIApplication application, NSUrl url, NSDictionary options)
    {
        AuthenticationContinuationHelper.SetAuthenticationContinuationEventArgs(url);

        return true;
    }

    private void RequestToastPermissions()
    {
        UNUserNotificationCenter.Current.RequestAuthorization(
            options: UNAuthorizationOptions.Alert | UNAuthorizationOptions.Badge | UNAuthorizationOptions.Sound,
            completionHandler: (granted, error) =>
            {
                // Do something if needed
            });
    }
}
