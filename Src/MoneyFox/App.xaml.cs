using CommonServiceLocator;
using MediatR;
using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using MoneyFox.Application;
using MoneyFox.Application.Common.Adapters;
using MoneyFox.Application.Common.CloudBackup;
using MoneyFox.Application.Common.Facades;
using MoneyFox.Application.Payments.Commands.ClearPayments;
using MoneyFox.Application.Payments.Commands.CreateRecurringPayments;
using MoneyFox.Services;
using NLog;
using PCLAppConfig;
using PCLAppConfig.FileSystemStream;
using System;
using System.Globalization;
using System.Threading.Tasks;
using Xamarin.Forms;


namespace MoneyFox
{
    public partial class App : Xamarin.Forms.Application
    {
        private readonly Logger logger = LogManager.GetCurrentClassLogger();

        public App()
        {
            Xamarin.Forms.Device.SetFlags(new [] {
                "AppTheme_Experimental",
                "SwipeView_Experimental"
            });

            App.Current.UserAppTheme = App.Current.RequestedTheme != OSAppTheme.Unspecified
                ? App.Current.RequestedTheme
                : OSAppTheme.Dark;

            App.Current.RequestedThemeChanged += (s, a) =>
            {
                App.Current.UserAppTheme = a.RequestedTheme;
            };

            CultureHelper.CurrentCulture = new CultureInfo(new SettingsFacade(new SettingsAdapter()).DefaultCulture);

            InitializeComponent();
            MainPage = new AppShell();
        }

        protected override async void OnStart()
        {
            if(ConfigurationManager.AppSettings == null)
            {
                ConfigurationManager.Initialise(PortableStream.Current);
            }

            InitializeAppCenter();
            ExecuteStartupTasks();

            var toaster = ServiceLocator.Current.GetInstance<IToastService>();
            await toaster.ShowToastAsync("Foo");
        }

        protected override void OnResume() => ExecuteStartupTasks();

        private static void InitializeAppCenter()
        {
            if(ConfigurationManager.AppSettings != null)
            {
                var iosAppCenterSecret = ConfigurationManager.AppSettings["IosAppcenterSecret"];
                var androidAppCenterSecret = ConfigurationManager.AppSettings["AndroidAppcenterSecret"];

                AppCenter.Start($"android={androidAppCenterSecret};" +
                                $"ios={iosAppCenterSecret}",
                                typeof(Analytics), typeof(Crashes));
            }
        }

        private void ExecuteStartupTasks()
        {
#pragma warning disable 4014
            Task.Run(async () =>
            {
                await StartupTasks();
            }).ConfigureAwait(false);
#pragma warning restore 4014

        }

        private async Task StartupTasks()
        {
            var settingsFacade = new SettingsFacade(new SettingsAdapter());

            var mediator = ServiceLocator.Current.GetInstance<IMediator>();
            if(!settingsFacade.IsBackupAutouploadEnabled || !settingsFacade.IsLoggedInToBackupService)
            {
                await mediator.Send(new ClearPaymentsCommand());
                await mediator.Send(new CreateRecurringPaymentsCommand());
                return;
            }

            try
            {
                var backupService = ServiceLocator.Current.GetInstance<IBackupService>();
                await backupService.RestoreBackupAsync();

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
            }
        }
    }
}
