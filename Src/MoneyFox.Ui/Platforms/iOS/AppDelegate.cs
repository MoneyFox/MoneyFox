// ReSharper disable once CheckNamespace
namespace MoneyFox.Ui;

using Foundation;
using JetBrains.Annotations;
using Microsoft.Identity.Client;
using SQLitePCL;
using UIKit;
using UserNotifications;

[Register("AppDelegate")]
[UsedImplicitly]
public class AppDelegate : MauiUIApplicationDelegate
{
    private const string MSAL_APPLICATION_ID = "00a3e4cd-b4b0-4730-be62-5fcf90a94a1d";
    private const string MSAL_URI = $"msal{MSAL_APPLICATION_ID}://auth";

    protected override MauiApp CreateMauiApp()
    {
        MauiProgram.AddPlatformServicesAction = AddServices;
        Batteries_V2.Init();
        RequestToastPermissions();

        return MauiProgram.CreateMauiApp();
    }

    private static void AddServices(IServiceCollection services)
    {
        var publicClientApplication = PublicClientApplicationBuilder.Create(MSAL_APPLICATION_ID)
            .WithRedirectUri(MSAL_URI)
            .WithIosKeychainSecurityGroup("com.microsoft.adalcache")
            .Build();

        services.AddSingleton(publicClientApplication);
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
            completionHandler: (_, _) =>
            {
                // Do something if needed
            });
    }
}
