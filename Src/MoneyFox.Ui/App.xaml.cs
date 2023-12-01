namespace MoneyFox.Ui;

using Aptabase.Maui;
using Common.Exceptions;
using CommunityToolkit.Mvvm.Messaging;
using Core.Common.Interfaces;
using Core.Common.Settings;
using Core.Features._Legacy_.Payments.ClearPayments;
using Core.Features.DbBackup;
using Core.Features.TransactionRecurrence;
using Domain.Aggregates.AccountAggregate;
using Domain.Exceptions;
using MediatR;
using Messages;
using Serilog;
using Views;
using Views.Setup;

public partial class App
{
    private readonly IAptabaseClient aptabaseClient;
    private readonly IBackupService backupService;
    private readonly IMediator mediator;
    private readonly ISettingsFacade settingsFacade;
    private readonly IAppDbContext appDbContext;
    private bool isRunning;

    public App(
        IServiceProvider serviceProvider,
        IAppDbContext appDbContext,
        IMediator mediator,
        ISettingsFacade settingsFacade,
        IBackupService backupService,
        IAptabaseClient aptabaseClient)
    {
        this.mediator = mediator;
        this.settingsFacade = settingsFacade;
        this.backupService = backupService;
        this.aptabaseClient = aptabaseClient;
        this.appDbContext = appDbContext;
        ServiceProvider = serviceProvider;
        InitializeComponent();
        FillResourceDictionary();
        appDbContext.MigrateDb();
        MigrateAccountsToLedgers();
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

    private void MigrateAccountsToLedgers()
    {
        var accounts = appDbContext.Accounts.ToList();
        foreach (var account in accounts)
        {
            var payments = appDbContext.Payments
                .Where(p => p.ChargedAccount.Id == account.Id || p.TargetAccount != null && p.TargetAccount.Id == account.Id)
                .ToList();

            var startingBalance = account.CurrentBalance;
            foreach (var payment in payments)
            {
                if (payment.Type == PaymentType.Expense || payment.Type == PaymentType.Transfer && payment.ChargedAccount.Id == account.Id)
                {
                    startingBalance += payment.Amount;
                }

                if (payment.Type == PaymentType.Income
                    || payment is { Type: PaymentType.Transfer, TargetAccount: not null } && payment.TargetAccount.Id == account.Id)
                {
                    startingBalance -= payment.Amount;
                }
            }
        }
    }
}
