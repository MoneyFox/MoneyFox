// ReSharper disable once CheckNamespace
namespace MoneyFox.Ui;

using Android.App;
using Android.Runtime;
using JetBrains.Annotations;
using Microsoft.Identity.Client;
using Serilog;

[Application]
[UsedImplicitly]
public class MainApplication : MauiApplication
{
    private const string MSAL_APPLICATION_ID = "00a3e4cd-b4b0-4730-be62-5fcf90a94a1d";
    private const string MSAL_URI = $"msal{MSAL_APPLICATION_ID}://auth";

    public MainApplication(nint handle, JniHandleOwnership ownership) : base(handle: handle, ownership: ownership) { }

    protected override MauiApp CreateMauiApp()
    {
        MauiProgram.AddPlatformServicesAction = AddServices;

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
        var publicClientApplication = PublicClientApplicationBuilder.Create(MSAL_APPLICATION_ID).WithRedirectUri(MSAL_URI).Build();
        services.AddSingleton(publicClientApplication);
    }

    private void HandleAndroidException(object? sender, RaiseThrowableEventArgs e)
    {
        Log.Fatal(exception: e.Exception, messageTemplate: "Application Terminating");
    }
}
