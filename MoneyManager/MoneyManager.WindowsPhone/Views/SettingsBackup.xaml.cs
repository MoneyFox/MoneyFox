using System;
using Windows.UI.Xaml.Navigation;
using Microsoft.Practices.ServiceLocation;
using MoneyManager.Business;
using MoneyManager.Business.ViewModels;
using MoneyManager.Common;
using MoneyManager.Foundation;

namespace MoneyManager.Views {
    public sealed partial class SettingsBackup {
        private readonly NavigationHelper _navigationHelper;

        public SettingsBackup() {
            InitializeComponent();

            _navigationHelper = new NavigationHelper(this);
        }

        public NavigationHelper NavigationHelper {
            get { return _navigationHelper; }
        }

        private BackupViewModel backupView {
            get { return ServiceLocator.Current.GetInstance<BackupViewModel>(); }
        }

        #region NavigationHelper registration

        protected override async void OnNavigatedTo(NavigationEventArgs e) {
            try {
                await backupView.LogInToOneDrive();
                await backupView.LoadBackupCreationDate();
                _navigationHelper.OnNavigatedTo(e);
            }
            catch (Exception ex) {
                InsightHelper.Report(ex);
            }
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e) {
            _navigationHelper.OnNavigatedFrom(e);
        }

        #endregion NavigationHelper registration
    }
}