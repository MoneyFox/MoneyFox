using CommonServiceLocator;
using MediatR;
using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using MoneyFox.Core._Pending_;
using MoneyFox.Core._Pending_.Common.Facades;
using MoneyFox.Core.Commands.Payments.ClearPayments;
using MoneyFox.Core.Commands.Payments.CreateRecurringPayments;
using MoneyFox.Core.Interfaces;
using MoneyFox.Mobile.Infrastructure.Adapters;
using NLog;
using PCLAppConfig;
using PCLAppConfig.FileSystemStream;
using System;
using System.Globalization;
using System.Threading.Tasks;
using Xamarin.Forms;
using Device = Xamarin.Forms.Device;

namespace MoneyFox
{
    public partial class App
    {
        private readonly Logger logger = LogManager.GetCurrentClassLogger();
        private bool isRunning;

        public App()
        {
            Device.SetFlags(new[] { "AppTheme_Experimental", "SwipeView_Experimental" });

            Current.UserAppTheme = Current.RequestedTheme != OSAppTheme.Unspecified
                ? Current.RequestedTheme
                : OSAppTheme.Dark;

            Current.RequestedThemeChanged += (s, a) =>
            {
                Current.UserAppTheme = a.RequestedTheme;
            };

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