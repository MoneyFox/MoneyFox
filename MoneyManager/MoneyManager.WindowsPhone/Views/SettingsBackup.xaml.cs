using System;
using Windows.UI.Xaml.Navigation;
using Microsoft.Practices.ServiceLocation;
using MoneyManager.Business.ViewModels;
using MoneyManager.Common;
using Xamarin;

namespace MoneyManager.Views {
    public sealed partial class SettingsBackup {
        private readonly NavigationHelper navigationHelper;

        public SettingsBackup() {
            InitializeComponent();

            navigationHelper = new NavigationHelper(this);
        }

        public NavigationHelper NavigationHelper {
            get { return navigationHelper; }
        }

        private BackupViewModel backupView {
            get { return ServiceLocator.Current.GetInstance<BackupViewModel>(); }
        }

        #region NavigationHelper registration

        protected override async void OnNavigatedTo(NavigationEventArgs e) {
            try {
                await backupView.LogInToOneDrive();
                await backupView.LoadBackupCreationDate();
                navigationHelper.OnNavigatedTo(e);
            } catch (Exception ex) {
                Insights.Report(ex, ReportSeverity.Error);
            }
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e) {
            navigationHelper.OnNavigatedFrom(e);
        }

        #endregion NavigationHelper registration
    }
}