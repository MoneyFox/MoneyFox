namespace MoneyFox.Ui;

using Aptabase.Maui;
using CommunityToolkit.Mvvm.Messaging;
using Core.Common.Interfaces;
using Core.Common.Settings;
using Core.Features._Legacy_.Payments.ClearPayments;
using Core.Features.DbBackup;
using Core.Features.TransactionRecurrence;
using Domain.Exceptions;
using MediatR;
using Messages;
using Microsoft.Maui.Controls.PlatformConfiguration;
using Microsoft.Maui.Controls.PlatformConfiguration.iOSSpecific;
using Serilog;
using Views.Setup;

public partial class App
{
    private readonly IAptabaseClient aptabaseClient;
    private readonly IBackupService backupService;
    private readonly IMediator mediator;
    private readonly ISettingsFacade settingsFacade;
    private bool isRunning;

    public App(
        IAppDbContext appDbContext,
        IMediator mediator,
        ISettingsFacade settingsFacade,
        IBackupService backupService,
        IAptabaseClient aptabaseClient,
        MainPageViewModel mainPageViewModel,
        WelcomeViewModel welcomeViewModel)
    {
        this.mediator = mediator;
        this.settingsFacade = settingsFacade;
        this.backupService = backupService;
        this.aptabaseClient = aptabaseClient;
        InitializeComponent();
        FillResourceDictionary();
        appDbContext.MigrateDb();
        mainPageViewModel.OnNavigatedAsync(null);
        MainPage = new DefaultNavigationPage(settingsFacade.IsSetupCompleted ? new MainPage(mainPageViewModel) : new WelcomePage(welcomeViewModel));
    }

    public static Dictionary<string, ResourceDictionary> ResourceDictionary { get; } = new();

    private void FillResourceDictionary()
    {
        foreach (var dictionary in Resources.MergedDictionaries)
        {
            var key = dictionary.Source.OriginalString.Split(';').First().Split('/').Last().Split('.').First();
            ResourceDictionary.Add(key: key, value: dictionary);
        }
    }

    protected override void OnStart()
    {
        aptabaseClient.TrackEvent("app_start");
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
        try
        {
            if (settingsFacade is { IsBackupAutoUploadEnabled: true, IsLoggedInToBackupService: true })
            {
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
            await mediator.Send(new CheckTransactionRecurrence.Command());
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
