namespace MoneyFox.Ui;

using Aptabase.Maui;
using Common.Exceptions;
using CommunityToolkit.Mvvm.Messaging;
using Core.Common.Extensions;
using Core.Common.Interfaces;
using Core.Common.Settings;
using Core.Features._Legacy_.Payments.ClearPayments;
using Core.Features.DbBackup;
using Core.Features.TransactionRecurrence;
using Domain.Aggregates.AccountAggregate;
using Domain.Aggregates.RecurringTransactionAggregate;
using Domain.Exceptions;
using MediatR;
using Messages;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Views;
using Views.Setup;

public partial class App
{
    private readonly IAppDbContext appDbContext;
    private readonly IAptabaseClient aptabaseClient;
    private readonly IBackupService backupService;
    private readonly IMediator mediator;
    private readonly ISettingsFacade settingsFacade;
    private bool isRunning;

    public App(
        IServiceProvider serviceProvider,
        IAppDbContext appDbContext,
        IMediator mediator,
        ISettingsFacade settingsFacade,
        IBackupService backupService,
        IAptabaseClient aptabaseClient)
    {
        this.appDbContext = appDbContext;
        this.mediator = mediator;
        this.settingsFacade = settingsFacade;
        this.backupService = backupService;
        this.aptabaseClient = aptabaseClient;
        ServiceProvider = serviceProvider;
        InitializeComponent();
        FillResourceDictionary();
        appDbContext.MigrateDb();
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
            await MigrateRecurringTransactions();
            if (settingsFacade.RecurringTransactionMigrated2)
            {
                await mediator.Send(new CheckTransactionRecurrence.Command());
            }

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

    private async Task MigrateRecurringTransactions()
    {
        // Migrate RecurringTransaction
        if (settingsFacade.RecurringTransactionMigrated2 is false)
        {
            appDbContext.RecurringTransactions.RemoveRange(await appDbContext.RecurringTransactions.ToListAsync());
            await appDbContext.SaveChangesAsync();
            var recurringPayments = await appDbContext.RecurringPayments.Include(rp => rp.Category)
                .Include(rp => rp.ChargedAccount)
                .Include(rp => rp.TargetAccount)
                .Include(rp => rp.RelatedPayments)
                .Where(rp => rp.LastRecurrenceCreated != DateTime.MinValue)
                .ToListAsync();

            foreach (var recurringPayment in recurringPayments)
            {
                var recurringTransactionId = Guid.NewGuid();
                var amount = recurringPayment.Type == PaymentType.Expense ? -recurringPayment.Amount : recurringPayment.Amount;
                var recurringTransaction = RecurringTransaction.Create(
                    recurringTransactionId: recurringTransactionId,
                    chargedAccount: recurringPayment.ChargedAccount.Id,
                    targetAccount: recurringPayment.TargetAccount?.Id,
                    amount: new(amount: amount, currencyAlphaIsoCode: settingsFacade!.DefaultCurrency),
                    categoryId: recurringPayment.Category?.Id,
                    startDate: recurringPayment.StartDate.ToDateOnly(),
                    endDate: recurringPayment.EndDate.HasValue ? DateOnly.FromDateTime(recurringPayment.EndDate.Value) : null,
                    recurrence: recurringPayment.Recurrence.ToRecurrence(),
                    note: recurringPayment.Note,
                    isLastDayOfMonth: recurringPayment.IsLastDayOfMonth,
                    lastRecurrence: new(
                        year: recurringPayment.LastRecurrenceCreated.Year,
                        month: recurringPayment.LastRecurrenceCreated.Month,
                        day: recurringPayment.StartDate.Day),
                    isTransfer: recurringPayment.Type == PaymentType.Transfer);

                foreach (var payment in recurringPayment.RelatedPayments)
                {
                    payment.AddRecurringTransaction(recurringTransactionId);
                }

                appDbContext.Add(recurringTransaction);
            }

            await appDbContext.SaveChangesAsync();
            settingsFacade.RecurringTransactionMigrated2 = true;
            settingsFacade.RecurringTransactionMigrated = true;
        }
    }
}
