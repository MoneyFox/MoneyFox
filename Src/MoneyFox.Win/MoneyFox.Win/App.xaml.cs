namespace MoneyFox.Win;

using System;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Common.Exceptions;
using Core.ApplicationCore.UseCases.DbBackup;
using Core.Commands.Payments.ClearPayments;
using Core.Commands.Payments.CreateRecurringPayments;
using Core.Common.Facades;
using Core.Common.Interfaces;
using InversionOfControl;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml;
using Pages;
using Pages.Accounts;
using Pages.Categories;
using Pages.Payments;
using Pages.Settings;
using Pages.Statistics;
using Pages.Statistics.StatisticCategorySummary;
using Serilog;
using Services;
using ViewModels;
using ViewModels.Accounts;
using ViewModels.Categories;
using ViewModels.DataBackup;
using ViewModels.Payments;
using ViewModels.Settings;
using ViewModels.Statistics;
using ViewModels.Statistics.StatisticCategorySummary;

public partial class App : Application
{
    private bool isRunning;

    public App()
    {
        LoggerService.Initialize();
        SetupServices();
        NavigationService.Register<ShellViewModel, ShellPage>();
        NavigationService.Register<AccountListViewModel, AccountListPage>();
        NavigationService.Register<PaymentListViewModel, PaymentListPage>();
        NavigationService.Register<AddPaymentViewModel, AddPaymentPage>();
        NavigationService.Register<EditPaymentViewModel, EditPaymentPage>();
        NavigationService.Register<CategoryListViewModel, CategoryListPage>();
        NavigationService.Register<SettingsViewModel, SettingsHostPage>();
        NavigationService.Register<StatisticCashFlowViewModel, StatisticCashFlowPage>();
        NavigationService.Register<StatisticCategoryProgressionViewModel, StatisticCategoryProgressionPage>();
        NavigationService.Register<StatisticAccountMonthlyCashflowViewModel, StatisticAccountMonthlyCashFlowPage>();
        NavigationService.Register<StatisticCategorySpreadingViewModel, StatisticCategorySpreadingPage>();
        NavigationService.Register<StatisticCategorySummaryViewModel, StatisticCategorySummaryPage>();
        NavigationService.Register<StatisticSelectorViewModel, StatisticSelectorPage>();
        NavigationService.Register<BackupViewModel, BackupPage>();
        NavigationService.Register<WindowsSettingsViewModel, SettingsHostPage>();
        InitializeComponent();
    }

    private static IServiceProvider? ServiceProvider { get; set; }

    internal static BaseViewModel GetViewModel<TViewModel>() where TViewModel : BaseViewModel
    {
        return ServiceProvider?.GetService<TViewModel>() ?? throw new ResolveViewModeException<TViewModel>();
    }

    private static void SetupServices()
    {
        var services = new ServiceCollection();
        new WindowsConfig().Register(services);
        ServiceProvider = services.BuildServiceProvider();
        ServiceProvider.GetService<IAppDbContext>()?.Migratedb();
    }

    protected override void OnLaunched(LaunchActivatedEventArgs args)
    {
        InitializeAppCenter();

        var mainWindow = new MainWindow();
        mainWindow.Activate();

        ExecuteStartupTasks();
    }

    private static void InitializeAppCenter()
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Package.Current.InstalledLocation.Path)
            .AddJsonFile("appsettings.json", optional: false)
            .Build();

        var appCenter = configuration.GetRequiredSection("AppCenter").Get<AppCenter>()!;

        Microsoft.AppCenter.AppCenter.Start(appSecret: appCenter.Secret,
            typeof(Microsoft.AppCenter.Analytics.Analytics), typeof(Microsoft.AppCenter.Crashes.Crashes));
    }

    private void ExecuteStartupTasks()
    {
        Task.Run(async () => { await StartupTasksAsync(); }).ConfigureAwait(false);
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
                var backupService = ServiceProvider.GetService<IBackupService>() ?? throw new ResolveDependencyException<IMediator>();
                await backupService.RestoreBackupAsync();
            }

            await mediator.Send(new ClearPaymentsCommand());
            await mediator.Send(new CreateRecurringPaymentsCommand());
        }
        catch (Exception ex)
        {
            Log.Fatal(exception: ex, messageTemplate: "Error during startup");
        }
        finally
        {
            settingsFacade.LastExecutionTimeStampSyncBackup = DateTime.Now;
            isRunning = false;
        }
    }
}
