namespace MoneyFox.iOS;

using Autofac;
using Core.Common;
using Foundation;
using JetBrains.Annotations;
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
        RegisterServices();
        RequestToastPermissions();

        return MauiProgram.CreateMauiApp();
    }

    // Needed for auth
    public override bool OpenUrl(UIApplication application, NSUrl url, NSDictionary options)
    {
        // TODO: Reactivate once Microsoft.Identity.Client fixed issue with NSUrl
        // AuthenticationContinuationHelper.SetAuthenticationContinuationEventArgs(url);

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

    private void RegisterServices()
    {
        var builder = new ContainerBuilder();
        builder.RegisterModule<IosModule>();
        ViewModelLocator.RegisterServices(builder);
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
