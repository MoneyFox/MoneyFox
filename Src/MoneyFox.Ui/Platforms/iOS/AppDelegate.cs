namespace MoneyFox.Ui.Platforms.iOS;
using Core.Common;
using Core.Common.Interfaces;
using Core.Interfaces;
using Foundation;
using JetBrains.Annotations;
using Microsoft.Identity.Client;
using MoneyFox.Ui;
using MoneyFox.Ui.Platforms.iOS.Src;
using Serilog;
using Serilog.Events;
using Serilog.Exceptions;
using UIKit;
using UserNotifications;

[Register("AppDelegate")]
[UsedImplicitly]
public class AppDelegate : MauiUIApplicationDelegate
{
    protected override MauiApp CreateMauiApp()
    {
        InitLogger();
        App.AddPlatformServicesAction = AddServices;
        RequestToastPermissions();

        return MauiProgram.CreateMauiApp();
    }

    private static void AddServices(IServiceCollection services)
    {
        services.AddSingleton<IDbPathProvider, DbPathProvider>();
        services.AddSingleton<IStoreOperations, StoreOperations>();
        services.AddSingleton<IAppInformation, AppInformation>();
        services.AddTransient<IFileStore>(_ => new IosFileStore(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)));
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

    private void InitLogger()
    {
        var logFile = Path.Combine(path1: FileSystem.AppDataDirectory, path2: LogConfiguration.FileName);
        Log.Logger = new LoggerConfiguration().MinimumLevel.Debug()
            .Enrich.FromLogContext()
            .Enrich.WithExceptionDetails()
            .WriteTo.File(path: logFile, rollingInterval: RollingInterval.Month, retainedFileCountLimit: 6, restrictedToMinimumLevel: LogEventLevel.Information)
            .CreateLogger();

        Log.Information("Application Startup");
    }
}
