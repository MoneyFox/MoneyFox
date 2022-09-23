namespace MoneyFox.iOS
{

    using System;
    using System.IO;
    using Core.Common;
    using Core.Common.Interfaces;
    using Core.Interfaces;
    using Foundation;
    using Infrastructure.DbBackup;
    using JetBrains.Annotations;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Identity.Client;
    using Serilog;
    using Serilog.Events;
    using Serilog.Exceptions;
    using UIKit;
    using UserNotifications;
    using Xamarin.Essentials;
    using Xamarin.Forms;
    using Xamarin.Forms.Platform.iOS;

    [Register("AppDelegate")]
    [UsedImplicitly]
    public class AppDelegate : FormsApplicationDelegate
    {
        // You have 17 seconds to return from this method, or iOS will terminate your application.
        //
        public override bool FinishedLaunching(UIApplication uiApplication, NSDictionary launchOptions)
        {
            InitLogger();
            Forms.Init();
            FormsMaterial.Init();
            LoadApplication(new App(AddServices));
            RequestToastPermissions();

            return base.FinishedLaunching(uiApplication: uiApplication, launchOptions: launchOptions);
        }

        private static void AddServices(IServiceCollection services)
        {
            services.AddSingleton<IDbPathProvider, DbPathProvider>();
            services.AddSingleton<IStoreOperations, StoreOperations>();
            services.AddSingleton<IAppInformation, AppInformation>();
            services.AddTransient<IFileStore>(_ => new IosFileStore(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)));
        }

        // Needed for auth
        public override bool OpenUrl(UIApplication app, NSUrl url, NSDictionary options)
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
                .WriteTo.File(
                    path: logFile,
                    rollingInterval: RollingInterval.Month,
                    retainedFileCountLimit: 6,
                    restrictedToMinimumLevel: LogEventLevel.Information)
                .CreateLogger();

            Log.Information("Application Startup");
        }
    }

}
