using CommonServiceLocator;
using MediatR;
using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using MoneyFox.Application;
using MoneyFox.Application.Common.Adapters;
using MoneyFox.Application.Common.Facades;
using MoneyFox.Application.DbBackup;
using MoneyFox.Application.Payments.Commands.ClearPayments;
using MoneyFox.Application.Payments.Commands.CreateRecurringPayments;
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
    public partial class App : Xamarin.Forms.Application
    {
        private readonly Logger logger = LogManager.GetCurrentClassLogger();
        private bool isRunning;

        public App()
        {
            Device.SetFlags(new[] {"AppTheme_Experimental", "SwipeView_Experimental"});

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
#pragma warning disable S4462 // Calls to "async" methods should not be blocking
                Shell.Current.GoToAsync(ViewModelLocator.WelcomeViewRoute).Wait();
#pragma warning restore S4462 // Calls to "async" methods should not be blocking
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

                AppCenter.Start($"android={androidAppCenterSecret};" +
                                $"ios={iosAppCenterSecret}",
                    typeof(Analytics), typeof(Crashes));
            }
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
                    IBackupService backupService = ServiceLocator.Current.GetInstance<IBackupService>();
                    await backupService.RestoreBackupAsync();
                }

                await mediator.Send(new ClearPaymentsCommand());
                await mediator.Send(new CreateRecurringPaymentsCommand());

                logger.Info("Backup synced.");
            }
            catch(Exception ex)
            {
                logger.Fatal(ex);
                throw;
            }
            finally
            {
                settingsFacade.LastExecutionTimeStampSyncBackup = DateTime.Now;
                isRunning = false;
            }
        }
    }
}