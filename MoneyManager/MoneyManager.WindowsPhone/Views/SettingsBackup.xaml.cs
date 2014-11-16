using Microsoft.Practices.ServiceLocation;
using MoneyManager.Business.ViewModels;
using MoneyManager.Common;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace MoneyManager.Views
{
    public sealed partial class SettingsBackup
    {
        private readonly NavigationHelper navigationHelper;

        public SettingsBackup()
        {
            InitializeComponent();

            navigationHelper = new NavigationHelper(this);
        }

        public NavigationHelper NavigationHelper
        {
            get { return navigationHelper; }
        }

        private BackupViewModel backupView
        {
            get { return ServiceLocator.Current.GetInstance<BackupViewModel>(); }
        }

        #region NavigationHelper registration

        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            await backupView.LogInToOneDrive();
            await backupView.LoadBackupCreationDate();
            navigationHelper.OnNavigatedTo(e);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            navigationHelper.OnNavigatedFrom(e);
        }

        #endregion NavigationHelper registration
    }
}