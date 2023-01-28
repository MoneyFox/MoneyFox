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
using Microsoft.Identity.Client;

[Application]
[UsedImplicitly]
public class MainApplication : MauiApplication
{
    private const string MSAL_APPLICATIONID = "00a3e4cd-b4b0-4730-be62-5fcf90a94a1d";
    private const string MSAL_URI = $"msal{MSAL_APPLICATIONID}://auth";

    public MainApplication(IntPtr handle, JniHandleOwnership ownership) : base(handle, ownership) { }

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
        services.AddSingleton<IAppInformation, DroidAppInformation>();
        services.AddTransient<IFileStore>(_ => new FileStoreIoBase(Context.FilesDir?.Path ?? ""));
        RegisterIdentityClient(services);
    }

    private static void RegisterIdentityClient(IServiceCollection serviceCollection)
    {
        IPublicClientApplication publicClientApplication = PublicClientApplicationBuilder.Create(MSAL_APPLICATIONID).WithRedirectUri(MSAL_URI).Build();
        serviceCollection.AddSingleton(publicClientApplication);
    }

    private void HandleAndroidException(object sender, RaiseThrowableEventArgs e)
    {
        Log.Fatal(exception: e.Exception, messageTemplate: "Application Terminating");
    }
}
