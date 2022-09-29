namespace MoneyFox.Ui;

using System;
using System.IO;
using Core.Common;
using Android.App;
using Android.Runtime;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Maui;
using Microsoft.Maui.Hosting;
using Microsoft.Maui.Storage;
using MoneyFox.Core.Common.Interfaces;
using MoneyFox.Core.Interfaces;
using MoneyFox.Ui.Common;
using MoneyFox.Ui.Platforms.Android.Resources.Src;
using Serilog;
using Serilog.Events;
using Serilog.Exceptions;

[Application]
[UsedImplicitly]
public class MainApplication : MauiApplication
{
    private static void AddServices(IServiceCollection services)
    {
        services.AddSingleton<IDbPathProvider, DbPathProvider>();
        services.AddSingleton<IStoreOperations, PlayStoreOperations>();
        services.AddSingleton<IAppInformation, DroidAppInformation>();
        services.AddTransient<IFileStore>(_ => new FileStoreIoBase(Context.FilesDir?.Path ?? ""));
    }

    public MainApplication(IntPtr handle, JniHandleOwnership ownership)
        : base(handle, ownership)
    {
    }

    protected override MauiApp CreateMauiApp()
    {
        App.AddPlatformServicesAction = AddServices;
        return MauiProgram.CreateMauiApp();
    }

    public override void OnCreate()
    {
        InitLogger();

        // Setup handler for uncaught exceptions.
        AndroidEnvironment.UnhandledExceptionRaiser += HandleAndroidException;
        base.OnCreate();
    }


    private void HandleAndroidException(object sender, RaiseThrowableEventArgs e)
    {
        Log.Fatal(exception: e.Exception, messageTemplate: "Application Terminating");
    }

    private void InitLogger()
    {
        Log.Logger = new LoggerConfiguration().MinimumLevel.Debug()
            .Enrich.FromLogContext()
            .Enrich.WithExceptionDetails()
            .WriteTo.File(
                path: Path.Combine(path1: FileSystem.AppDataDirectory, path2: LogConfiguration.FileName),
                rollingInterval: RollingInterval.Month,
                retainedFileCountLimit: 6,
                restrictedToMinimumLevel: LogEventLevel.Information)
            .CreateLogger();

        Log.Information("Application Startup");
    }
}

