using System;
using CommonServiceLocator;
using MoneyFox.Application.Common.CloudBackup;
using MoneyFox.Application.Common.Facades;
using MoneyFox.Application.Common.Interfaces;
using MoneyFox.Application.Resources;
using NLog;
using Xamarin.Forms;

namespace MoneyFox.Presentation.Views
{
    public partial class BackgroundJobSettingsPage
    {
        private Logger logger = LogManager.GetCurrentClassLogger();

        public BackgroundJobSettingsPage()
        {
            InitializeComponent();
            BindingContext = ViewModelLocator.SettingsBackgroundVm;
        }

        protected override void OnAppearing()
        {
            Title = Strings.BackgroundJobTitle;
            base.OnAppearing();
        }

        private async void Switch_OnToggled(object sender, ToggledEventArgs e)
        {
            var settingsFacade = ServiceLocator.Current.GetInstance<ISettingsFacade>();
            var backupService = ServiceLocator.Current.GetInstance<IBackupService>();
            var dialogService = ServiceLocator.Current.GetInstance<IDialogService>();

            if (e.Value && !settingsFacade.IsLoggedInToBackupService)
            {
                try
                {
                    await backupService.LoginAsync();
                }
                catch (Exception ex)
                {
                    BackgroundJobToggle.IsToggled = false;
                    logger.Error(ex);
                }
            }
        }
    }
}
