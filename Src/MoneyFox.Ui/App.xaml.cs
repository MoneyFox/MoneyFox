namespace MoneyFox.Ui;

using Common.Exceptions;
using CommunityToolkit.Mvvm.Messaging;
using Core.Common.Interfaces;
using Core.Common.Settings;
using Core.Features._Legacy_.Payments.ClearPayments;
using Core.Features._Legacy_.Payments.CreateRecurringPayments;
using Core.Features.DbBackup;
using Domain.Exceptions;
using Infrastructure.Adapters;
using MediatR;
using Messages;
using Serilog;
using Views;
using Views.Setup;

public partial class App
{
    private bool isRunning;

    public App(IServiceProvider serviceProvider)
    {
        ServiceProvider = serviceProvider;
        var settingsAdapter = new SettingsAdapter();
        var settingsFacade = new SettingsFacade(settingsAdapter);
        InitializeComponent();
        FillResourceDictionary();
        MainPage = settingsFacade.IsSetupCompleted ? GetAppShellPage() : new SetupShell();
    }

    public static Dictionary<string, ResourceDictionary> ResourceDictionary { get; } = new();

    private static IServiceProvider ServiceProvider { get; set; }

    public static Page GetAppShellPage()
    {
        return DeviceInfo.Current.Idiom == DeviceIdiom.Desktop || DeviceInfo.Current.Idiom == DeviceIdiom.Tablet || DeviceInfo.Current.Idiom == DeviceIdiom.TV
            ? new AppShellDesktop()
            : new AppShell();
    }

    private void FillResourceDictionary()
    {
        foreach (var dictionary in Resources.MergedDictionaries)
        {
            var key = dictionary.Source.OriginalString.Split(';').First().Split('/').Last().Split('.').First();
            ResourceDictionary.Add(key: key, value: dictionary);
        }
    }

    internal static TViewModel GetViewModel<TViewModel>() where TViewModel : BasePageViewModel
    {
        return ServiceProvider.GetService<TViewModel>() ?? throw new ResolveViewModelException<TViewModel>();
    }

    protected override void OnStart()
    {
        StartupTasksAsync().ConfigureAwait(false);
    }

    protected override void OnResume()
    {
        StartupTasksAsync().ConfigureAwait(false);
    }

    private async Task StartupTasksAsync()
    {
        // Don't execute this again when already running
        if (isRunning)
        {
            return;
        }

        isRunning = true;
        ServiceProvider.GetService<IAppDbContext>()?.MigrateDb();
        var settingsFacade = ServiceProvider.GetService<ISettingsFacade>() ?? throw new ResolveDependencyException<ISettingsFacade>();
        var mediator = ServiceProvider.GetService<IMediator>() ?? throw new ResolveDependencyException<IMediator>();
        try
        {
            if (settingsFacade is { IsBackupAutoUploadEnabled: true, IsLoggedInToBackupService: true })
            {
                var backupService = ServiceProvider.GetService<IBackupService>() ?? throw new ResolveDependencyException<IBackupService>();
                await backupService.RestoreBackupAsync();
                WeakReferenceMessenger.Default.Send(new BackupRestoredMessage());
            }
        }
        catch (NetworkConnectionException)
        {
            Log.Information("Backup wasn't able to restore on startup - app is offline");
        }
        catch (Exception ex)
        {
            Log.Error(exception: ex, messageTemplate: "Failed to restore backup on startup");
        }

        try
        {
            await mediator.Send(new ClearPaymentsCommand());
            await mediator.Send(new CreateRecurringPaymentsCommand());
            settingsFacade.LastExecutionTimeStampSyncBackup = DateTime.Now;
        }
        catch (Exception ex)
        {
            Log.Error(exception: ex, messageTemplate: "Startup tasks failed");
        }
        finally
        {
            isRunning = false;
        }
    }
}
