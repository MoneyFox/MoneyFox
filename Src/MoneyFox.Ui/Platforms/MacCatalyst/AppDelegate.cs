namespace MoneyFox.Ui;

using Foundation;
using Microsoft.Identity.Client;
using UIKit;
using UserNotifications;

[Register("AppDelegate")]
public class AppDelegate : MauiUIApplicationDelegate
{
    private const string MSAL_APPLICATION_ID = "00a3e4cd-b4b0-4730-be62-5fcf90a94a1d";
    private const string MSAL_URI = $"msal{MSAL_APPLICATION_ID}://auth";

    protected override MauiApp CreateMauiApp()
    {
        MauiProgram.AddPlatformServicesAction = AddServices;
        RequestToastPermissions();

        return MauiProgram.CreateMauiApp();
    }

    private static void AddServices(IServiceCollection services)
    {
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
