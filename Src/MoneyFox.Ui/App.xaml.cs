namespace MoneyFox.Ui;

using Common.Exceptions;
using CommunityToolkit.Mvvm.Messaging;
using Core.Common.Interfaces;
using Core.Common.Settings;
using Core.Features._Legacy_.Payments.ClearPayments;
using Core.Features._Legacy_.Payments.CreateRecurringPayments;
using Core.Features.DbBackup;
using Core.Interfaces;
using Domain.Aggregates.BudgetAggregate;
using Infrastructure.Adapters;
using InversionOfControl;
using MediatR;
using Messages;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Views;
using Views.Setup;

public partial class App
{
    private const string IS_CATEGORY_CLEANUP_EXECUTED_KEY_NAME = "IsCategoryCleanupExecuted";
    private const string IS_SETUP_RESET_KEY_NAME = "IsSetipReset_8.1.14591";
    private const string IS_BUDGET_MIGRATION_DONE_KEY_NAME = "IsBudgetMigrationDone";
    private bool isRunning;

    public App()
    {
        var settingsAdapter = new SettingsAdapter();
        var settingsFacade = new SettingsFacade(settingsAdapter);
        InitializeComponent();
        SetupServices();
        FillResourceDictionary();

        ResetSetup(settingsAdapter);
        if (settingsFacade.IsSetupCompleted is false)
        {
            MainPage = new SetupShell2();
        }
        else
        {
            MainPage = GetAppShellPage();
        }

        FixCorruptPayments(settingsAdapter);
        MigrateBudgetData(settingsAdapter);
    }

    public static Page GetAppShellPage()
    {
        return DeviceInfo.Current.Idiom == DeviceIdiom.Desktop
               || DeviceInfo.Current.Idiom == DeviceIdiom.Tablet
               || DeviceInfo.Current.Idiom == DeviceIdiom.TV
            ? new AppShellDesktop()
            : new AppShell();
    }

    public static Dictionary<string, ResourceDictionary> ResourceDictionary { get; } = new();

    public static Action<IServiceCollection>? AddPlatformServicesAction { get; set; }

    private static IServiceProvider? ServiceProvider { get; set; }

    private static void ResetSetup(ISettingsAdapter settingsAdapter)
    {
        try
        {
            if (settingsAdapter.GetValue(key: IS_SETUP_RESET_KEY_NAME, defaultValue: false))
            {
                return;
            }

            if (ServiceProvider?.GetService<ISettingsFacade>() == null)
            {
                return;
            }

            var settingsFacade = ServiceProvider.GetService<ISettingsFacade>();
            settingsFacade!.IsSetupCompleted = false;
            settingsAdapter.AddOrUpdate(key: IS_SETUP_RESET_KEY_NAME, true);
        }
        catch (Exception ex)
        {
            Log.Error(exception: ex, messageTemplate: "Error while reseting setup");
            Crashes.TrackError(ex);
        }
    }

    /// <summary>
    ///     This removes the link from payments to categories that no longer exists.
    ///     https://github.com/MoneyFox/MoneyFox/issues/2717
    ///     This can be removed in a future release.
    /// </summary>
    private static void FixCorruptPayments(ISettingsAdapter settingsAdapter)
    {
        try
        {
            if (settingsAdapter.GetValue(key: IS_CATEGORY_CLEANUP_EXECUTED_KEY_NAME, defaultValue: false))
            {
                return;
            }

            if (ServiceProvider?.GetService<IAppDbContext>() == null)
            {
                return;
            }

            var dbContext = ServiceProvider.GetService<IAppDbContext>();
            var categoryIds = dbContext!.Categories.Select(c => c.Id).ToList();
            var paymentsWithCategory = dbContext.Payments.Include(p => p.Category).Where(p => p.Category != null).ToList();
            foreach (var payment in paymentsWithCategory.Where(payment => categoryIds.Contains(payment.Category!.Id) is false))
            {
                payment.RemoveCategory();
                Analytics.TrackEvent(nameof(FixCorruptPayments));
            }

            dbContext.SaveChangesAsync().GetAwaiter().GetResult();
            settingsAdapter.AddOrUpdate(key: IS_CATEGORY_CLEANUP_EXECUTED_KEY_NAME, true);
        }
        catch (Exception ex)
        {
            Log.Error(exception: ex, messageTemplate: "Error while fixing payment with non existing category");
            Crashes.TrackError(ex);
        }
    }

    /// <summary>
    ///     This removes the link from payments to categories that no longer exists.
    ///     https://github.com/MoneyFox/MoneyFox/issues/2717
    ///     This can be removed in a future release.
    /// </summary>
    private static void MigrateBudgetData(ISettingsAdapter settingsAdapter)
    {
        try
        {
            if (ServiceProvider?.GetService<IAppDbContext>() == null)
            {
                return;
            }

            var dbContext = ServiceProvider.GetService<IAppDbContext>();
            var budgets = dbContext!.Budgets.ToList();
            if (budgets.All(b => b.Interval != 0))
            {
                return;
            }

            foreach (var budget in budgets)
            {
                switch (budget.BudgetTimeRange)
                {
                    case BudgetTimeRange.YearToDate:
                        budget.SetInterval(1);

                        break;
                    case BudgetTimeRange.Last1Year:
                        budget.SetInterval(12);

                        break;
                    case BudgetTimeRange.Last2Years:
                        budget.SetInterval(24);

                        break;
                    case BudgetTimeRange.Last3Years:
                        budget.SetInterval(36);

                        break;
                    case BudgetTimeRange.Last5Years:
                        budget.SetInterval(60);

                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            dbContext.SaveChangesAsync().GetAwaiter().GetResult();
            settingsAdapter.AddOrUpdate(key: IS_BUDGET_MIGRATION_DONE_KEY_NAME, true);
        }
        catch (Exception ex)
        {
            Log.Error(exception: ex, messageTemplate: "Error while migrating budget to interval");
            Crashes.TrackError(ex);
        }
    }

    private void FillResourceDictionary()
    {
        foreach (var dictionary in Resources.MergedDictionaries)
        {
            var key = dictionary.Source.OriginalString.Split(';').First().Split('/').Last().Split('.').First();
            ResourceDictionary.Add(key: key, dictionary);
        }
    }

    internal static TViewModel GetViewModel<TViewModel>() where TViewModel : BasePageViewModel
    {
        return ServiceProvider?.GetService<TViewModel>() ?? throw new ResolveViewModelException<TViewModel>();
    }

    protected override void OnStart()
    {
        StartupTasksAsync().ConfigureAwait(false);
    }

    protected override void OnResume()
    {
        StartupTasksAsync().ConfigureAwait(false);
    }

    private static void SetupServices()
    {
        var services = new ServiceCollection();
        AddPlatformServicesAction?.Invoke(services);
        new MoneyFoxConfig().Register(services);
        ServiceProvider = services.BuildServiceProvider();
        ServiceProvider.GetService<IAppDbContext>()?.MigrateDb();
    }

    private async Task StartupTasksAsync()
    {
        // Don't execute this again when already running
        if (isRunning)
        {
            return;
        }

        if (ServiceProvider == null)
        {
            return;
        }

        isRunning = true;
        var settingsFacade = ServiceProvider.GetService<ISettingsFacade>() ?? throw new ResolveDependencyException<ISettingsFacade>();
        var mediator = ServiceProvider.GetService<IMediator>() ?? throw new ResolveDependencyException<IMediator>();
        try
        {
            if (settingsFacade.IsBackupAutoUploadEnabled && settingsFacade.IsLoggedInToBackupService)
            {
                var backupService = ServiceProvider.GetService<IBackupService>() ?? throw new ResolveDependencyException<IBackupService>();
                await backupService.RestoreBackupAsync();
                WeakReferenceMessenger.Default.Send(new BackupRestoredMessage());
            }

            await mediator.Send(new ClearPaymentsCommand());
            await mediator.Send(new CreateRecurringPaymentsCommand());
            settingsFacade.LastExecutionTimeStampSyncBackup = DateTime.Now;
        }
        catch (Exception ex)
        {
            Log.Fatal(exception: ex, messageTemplate: "Error during startup");
        }
        finally
        {
            isRunning = false;
        }
    }
}
