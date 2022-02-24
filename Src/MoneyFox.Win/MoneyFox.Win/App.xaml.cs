namespace MoneyFox.Win
{
    using Autofac;
    using CommonServiceLocator;
    using MediatR;
    using Microsoft.UI.Xaml;
    using MoneyFox.Core._Pending_.Common.Facades;
    using MoneyFox.Core.Commands.Payments.ClearPayments;
    using MoneyFox.Core.Commands.Payments.CreateRecurringPayments;
    using MoneyFox.Core.Interfaces;
    using NLog;
    using System;
    using System.Threading.Tasks;

#if !DEBUG
    using Microsoft.AppCenter;
    using Microsoft.AppCenter.Analytics;
    using Microsoft.AppCenter.Crashes;
#endif

    public partial class App : Application
    {
        private readonly Logger logger = LogManager.GetCurrentClassLogger();
        private bool isRunning;

        public App()
        {
            InitializeComponent();
        }

        protected override async void OnLaunched(Microsoft.UI.Xaml.LaunchActivatedEventArgs args)
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
            Task.Run(
                    async () =>
                    {
                        await StartupTasksAsync();
                    })
                .ConfigureAwait(false);

        private async Task StartupTasksAsync()
        {
            // Don't execute this again when already running
            if(isRunning)
            {
                return;
            }

            isRunning = true;

            var settingsFacade = ServiceLocator.Current.GetInstance<ISettingsFacade>();
            var mediator = ServiceLocator.Current.GetInstance<IMediator>();

            try
            {
                if(settingsFacade.IsBackupAutouploadEnabled && settingsFacade.IsLoggedInToBackupService)
                {
                    var backupService = ServiceLocator.Current.GetInstance<IBackupService>();
                    await backupService.RestoreBackupAsync();

                    logger.Info("Backup synced.");
                }

                await mediator.Send(new ClearPaymentsCommand());
                await mediator.Send(new CreateRecurringPaymentsCommand());
            }
            catch(Exception ex)
            {
                logger.Fatal(ex);
            }
            finally
            {
                settingsFacade.LastExecutionTimeStampSyncBackup = DateTime.Now;
                isRunning = false;
            }
        }
    }
}
