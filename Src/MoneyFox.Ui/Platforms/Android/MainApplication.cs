// ReSharper disable once CheckNamespace
namespace MoneyFox.Ui;

using Android.App;
using Android.Runtime;
using JetBrains.Annotations;
using Microsoft.Identity.Client;
using Serilog;

[Application]
[UsedImplicitly]
public class MainApplication(nint handle, JniHandleOwnership ownership) : MauiApplication(handle: handle, ownership: ownership)
{
    private const string MSAL_APPLICATION_ID = "00a3e4cd-b4b0-4730-be62-5fcf90a94a1d";
    private const string MSAL_URI = $"msal{MSAL_APPLICATION_ID}://auth";

    protected override MauiApp CreateMauiApp()
    {
        CreatePersonalFolder();
        MigrateAndroidDatabaseFile();
        MauiProgram.AddPlatformServicesAction = AddServices;

        return MauiProgram.CreateMauiApp();
    }

    private static void CreatePersonalFolder()
    {
        // This seems to be necessary with .net 8 since the folder does not exist
        var documentDirectoryPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
        if (Directory.Exists(documentDirectoryPath) is false)
        {
            Directory.CreateDirectory(documentDirectoryPath);
        }
    }

    /// <summary>
    ///     Migrates the db file, since there was a breaking change in .net 8 in the paths returned by special folder
    /// </summary>
    private static void MigrateAndroidDatabaseFile()
    {
        var databaseName = "moneyfox3.db";
        var oldDbPath = Path.Combine(path1: Environment.GetEnvironmentVariable("HOME") ?? "", path2: databaseName);
        if (File.Exists(oldDbPath))
        {
            var dbPath = Path.Combine(path1: Environment.GetFolderPath(Environment.SpecialFolder.Personal), path2: databaseName);
            File.Move(sourceFileName: oldDbPath, destFileName: dbPath);
        }
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
