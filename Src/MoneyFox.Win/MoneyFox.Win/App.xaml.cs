namespace MoneyFox.Win;

using System;
using System.Threading.Tasks;
using Autofac;
using Common.Exceptions;
using CommonServiceLocator;
using Core._Pending_.Common.Facades;
using Core.ApplicationCore.UseCases.BackupUpload;
using Core.ApplicationCore.UseCases.DbBackup;
using Core.Commands.Payments.ClearPayments;
using Core.Commands.Payments.CreateRecurringPayments;
using Core.Common.Interfaces;
using Core.Resources;
using InversionOfControl;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml;
using MoneyFox.Win.Pages;
using MoneyFox.Win.Pages.Accounts;
using MoneyFox.Win.Pages.Categories;
using MoneyFox.Win.Pages.Payments;
using MoneyFox.Win.Pages.Settings;
using MoneyFox.Win.Pages.Statistics;
using MoneyFox.Win.Pages.Statistics.StatisticCategorySummary;
using MoneyFox.Win.ViewModels.Accounts;
using MoneyFox.Win.ViewModels.Categories;
using MoneyFox.Win.ViewModels.DataBackup;
using MoneyFox.Win.ViewModels.Payments;
using MoneyFox.Win.ViewModels.Settings;
using MoneyFox.Win.ViewModels.Statistics;
using MoneyFox.Win.ViewModels.Statistics.StatisticCategorySummary;
using Serilog;
using Services;
using ViewModels;

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
    }

    protected override void OnLaunched(LaunchActivatedEventArgs args)
    {
        var mainWindow = new MainWindow();
        mainWindow.Activate();
        ExecuteStartupTasks();
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

        isRunning = true;
        var toastService = ServiceLocator.Current.GetInstance<IToastService>();
        var settingsFacade = ServiceLocator.Current.GetInstance<ISettingsFacade>();
        var mediator = ServiceLocator.Current.GetInstance<IMediator>();

        try
        {
            if (settingsFacade.IsBackupAutoUploadEnabled && settingsFacade.IsLoggedInToBackupService)
            {
                var backupService = ServiceLocator.Current.GetInstance<IBackupService>();
                await backupService.RestoreBackupAsync();
            }

            await mediator.Send(new ClearPaymentsCommand());
            await mediator.Send(new CreateRecurringPaymentsCommand());

            var uploadResult = await mediator.Send(new UploadBackup.Command());
            if (uploadResult == UploadBackup.UploadResult.Successful)
            {
                await toastService.ShowToastAsync(Strings.BackupCreatedMessage);
            }
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
