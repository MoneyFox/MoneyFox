namespace MoneyFox
{

    using System;
    using System.Globalization;
    using System.Threading.Tasks;
    using CommonServiceLocator;
    using Core._Pending_.Common.Facades;
    using Core.ApplicationCore.UseCases.BackupUpload;
    using Core.ApplicationCore.UseCases.DbBackup;
    using Core.Commands.Payments.ClearPayments;
    using Core.Commands.Payments.CreateRecurringPayments;
    using Core.Common;
    using Core.Common.Interfaces;
    using Core.Resources;
    using MediatR;
    using Mobile.Infrastructure.Adapters;
    using Serilog;
    using Xamarin.Forms;

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
            if (!settingsFacade.IsSetupCompleted)
            {
                Shell.Current.GoToAsync(ViewModelLocator.WelcomeViewRoute).Wait();
            }
        }

        protected override void OnStart()
        {
            StartupTasksAsync().ConfigureAwait(false);
        }

        protected override void OnResume()
        {
            StartupTasksAsync().ConfigureAwait(false);
        }

        protected override async void OnSleep()
        {
            await StartupTasksAsync();
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

}
