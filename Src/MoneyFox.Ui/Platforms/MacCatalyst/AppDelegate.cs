namespace MoneyFox.Ui;

using Core.Interfaces;
using Foundation;
using Microsoft.Identity.Client;
using Platforms.MacCatalyst.Src;
using UIKit;
using UserNotifications;

[Register("AppDelegate")]
public class AppDelegate : MauiUIApplicationDelegate
{
    private const string MSAL_APPLICATION_ID = "20e90083-34d6-4eab-8ba1-6b96fbc448c3";
    private const string MSAL_URI = $"msal{MSAL_APPLICATION_ID}://auth";

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
        var publicClientApplication = PublicClientApplicationBuilder.Create(MSAL_APPLICATION_ID)
            .WithRedirectUri(MSAL_URI)
            .WithIosKeychainSecurityGroup("com.microsoft.adalcache")
            .Build();

        _ = serviceCollection.AddSingleton(publicClientApplication);
    }

    // Needed for auth
    public override bool OpenUrl(UIApplication application, NSUrl url, NSDictionary options)
    {
        // AuthenticationContinuationHelper.SetAuthenticationContinuationEventArgs(url);

        return true;
    }

    private void RequestToastPermissions()
    {
        UNUserNotificationCenter.Current.RequestAuthorization(
            options: UNAuthorizationOptions.Alert | UNAuthorizationOptions.Badge | UNAuthorizationOptions.Sound,
            completionHandler: (_, _) =>
            {
                // Do something if needed
            });
    }
}
