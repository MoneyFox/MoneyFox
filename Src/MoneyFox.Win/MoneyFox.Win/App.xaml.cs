namespace MoneyFox.Win;

using System;
using System.Threading.Tasks;
using Autofac;
using CommonServiceLocator;
using Core._Pending_.Common.Facades;
using Core.Commands.Payments.ClearPayments;
using Core.Commands.Payments.CreateRecurringPayments;
using Core.Interfaces;
using MediatR;
using Microsoft.UI.Xaml;
using MoneyFox.Win.Services;
using Serilog;

#if !DEBUG
    using Microsoft.AppCenter;
    using Microsoft.AppCenter.Analytics;
    using Microsoft.AppCenter.Crashes;
#endif

public partial class App : Application
{
    private bool isRunning;

    public App()
    {
        LoggerService.Initialize();
        InitializeComponent();
    }

    protected override void OnLaunched(LaunchActivatedEventArgs args)
    {
        var builder = new ContainerBuilder();
        builder.RegisterModule<WindowsModule>();
        ViewModelLocator.RegisterServices(builder);

        var m_window = new MainWindow();
        m_window.Activate();

#if !DEBUG
            var appConfig = new AppConfig();
            AppCenter.Start(appConfig.AppCenter.Secret, typeof(Analytics), typeof(Crashes));
#endif
        ExecuteStartupTasks();
    }

    private void ExecuteStartupTasks() =>
        Task.Run(async () =>
            {
                await StartupTasksAsync();
            })
            .ConfigureAwait(false);

    private async Task StartupTasksAsync()
    {
        // Don't execute this again when already running
        if (isRunning)
        {
            return;
        }

        isRunning = true;

        var settingsFacade = ServiceLocator.Current.GetInstance<ISettingsFacade>();
        var mediator = ServiceLocator.Current.GetInstance<IMediator>();

        try
        {
            if (settingsFacade.IsBackupAutouploadEnabled && settingsFacade.IsLoggedInToBackupService)
            {
                var backupService = ServiceLocator.Current.GetInstance<IBackupService>();
                await backupService.RestoreBackupAsync();
            }

            await mediator.Send(new ClearPaymentsCommand());
            await mediator.Send(new CreateRecurringPaymentsCommand());
        }
        catch (Exception ex)
        {
            Log.Fatal(ex, "Error during startup tasks");
        }
        finally
        {
            settingsFacade.LastExecutionTimeStampSyncBackup = DateTime.Now;
            isRunning = false;
        }
    }
}
