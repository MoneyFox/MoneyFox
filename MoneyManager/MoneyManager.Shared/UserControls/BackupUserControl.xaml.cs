#region

using System;
using Windows.UI.Xaml;
using Microsoft.Practices.ServiceLocation;
using MoneyManager.Business.ViewModels;

#endregion

namespace MoneyManager.UserControls
{
    public sealed partial class BackupUserControl
    {
        public BackupUserControl()
        {
            InitializeComponent();
        }

        private BackupViewModel backupView
        {
            get { return ServiceLocator.Current.GetInstance<BackupViewModel>(); }
        }

        private void LoginToOneDrive(object sender, RoutedEventArgs e)
        {
            backupView.LogInToOneDrive();
        }

        private void CreateBackup(object sender, RoutedEventArgs e)
        {
            backupView.CreateBackup();
        }

        private void RestoreBackup(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}