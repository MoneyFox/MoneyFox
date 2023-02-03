// ReSharper disable once CheckNamespace
namespace MoneyFox.Ui;

using Android.App;
using Android.Runtime;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Maui;
using Microsoft.Maui.Hosting;
using Core.Interfaces;
using Common;
using MoneyFox.Ui.Platforms.Android.Resources.Src;
using Serilog;
using Microsoft.Identity.Client;

[Application]
[UsedImplicitly]
public class MainApplication : MauiApplication
{
    private const string MSAL_APPLICATION_ID = "00a3e4cd-b4b0-4730-be62-5fcf90a94a1d";
    private const string MSAL_URI = $"msal{MSAL_APPLICATION_ID}://auth";

    public MainApplication(nint handle, JniHandleOwnership ownership) : base(handle, ownership) { }

    protected override MauiApp CreateMauiApp()
    {
        App.AddPlatformServicesAction = AddServices;

        return MauiProgram.CreateMauiApp();
    }

    public override void OnCreate()
    {
        // Setup handler for uncaught exceptions.
        AndroidEnvironment.UnhandledExceptionRaiser += HandleAndroidException;
        base.OnCreate();
    }

    private static void AddServices(IServiceCollection services)
    {
        services.AddSingleton<IDbPathProvider, DbPathProvider>();
        services.AddTransient<IFileStore>(_ => new FileStoreIoBase(Context.FilesDir?.Path ?? ""));
        RegisterIdentityClient(services);
    }

    private static void RegisterIdentityClient(IServiceCollection serviceCollection)
    {
        var publicClientApplication = PublicClientApplicationBuilder.Create(MSAL_APPLICATION_ID).WithRedirectUri(MSAL_URI).Build();
        serviceCollection.AddSingleton(publicClientApplication);
    }

    private void HandleAndroidException(object? sender, RaiseThrowableEventArgs e)
    {
        Log.Fatal(exception: e.Exception, messageTemplate: "Application Terminating");
    }
}
