using System;
using Windows.UI.Xaml;
using Microsoft.Practices.ServiceLocation;
using MoneyManager.Business.ViewModels;

namespace MoneyManager.UserControls
{
    public sealed partial class BackupUserControl
    {
        public BackupUserControl()
        {
            InitializeComponent();
        }

        private void LoginToOneDrive(object sender, RoutedEventArgs e)
        {
            ServiceLocator.Current.GetInstance<BackupViewModel>().LogInToOneDrive();
        }

        private void CreateBackup(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void RestoreBackup(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}
