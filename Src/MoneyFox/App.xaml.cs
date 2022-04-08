namespace MoneyFox
{
    using CommonServiceLocator;
    using Core._Pending_;
    using Core._Pending_.Common.Facades;
    using Core.Commands.Payments.ClearPayments;
    using Core.Commands.Payments.CreateRecurringPayments;
    using Core.Interfaces;
    using MediatR;
    using Microsoft.AppCenter;
    using Microsoft.AppCenter.Analytics;
    using Microsoft.AppCenter.Crashes;
    using Mobile.Infrastructure.Adapters;
    using NLog;
    using PCLAppConfig;
    using PCLAppConfig.FileSystemStream;
    using System;
    using System.Globalization;
    using System.Threading.Tasks;
    using Core.Common;
    using Serilog;
    using Xamarin.Forms;
    using Device = Xamarin.Forms.Device;

    public partial class App
    {
        private bool isRunning;

        public App()
        {
            Device.SetFlags(new[] { "AppTheme_Experimental", "SwipeView_Experimental" });

            var settingsFacade = new SettingsFacade(new SettingsAdapter());
            CultureHelper.CurrentCulture = new CultureInfo(settingsFacade.DefaultCulture);

            InitializeComponent();
            MainPage = new AppShell();

            if(!settingsFacade.IsSetupCompleted)
            {
                Shell.Current.GoToAsync(ViewModelLocator.WelcomeViewRoute).Wait();
            }
        }

        protected override void OnStart()
        {
            if(ConfigurationManager.AppSettings == null)
            {
                ConfigurationManager.Initialise(PortableStream.Current);
            }

            InitializeAppCenter();
            ExecuteStartupTasks();
        }

        protected override void OnResume() => ExecuteStartupTasks();

        private static void InitializeAppCenter()
        {
            if(ConfigurationManager.AppSettings != null)
            {
                string? iosAppCenterSecret = ConfigurationManager.AppSettings["IosAppcenterSecret"];
                string? androidAppCenterSecret = ConfigurationManager.AppSettings["AndroidAppcenterSecret"];

                AppCenter.Start(
                    $"android={androidAppCenterSecret};" + $"ios={iosAppCenterSecret}",
                    typeof(Analytics),
                    typeof(Crashes));
            }
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
                }

                await mediator.Send(new ClearPaymentsCommand());
                await mediator.Send(new CreateRecurringPaymentsCommand());
            }
            catch(Exception ex)
            {
                Log.Fatal(ex, "Error during startup");
            }
            finally
            {
                settingsFacade.LastExecutionTimeStampSyncBackup = DateTime.Now;
                isRunning = false;
            }
        }
    }
}
