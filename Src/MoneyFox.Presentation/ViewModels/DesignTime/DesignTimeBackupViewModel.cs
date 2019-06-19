using System;
using System.Globalization;
using GalaSoft.MvvmLight.Command;
using MoneyFox.Foundation.Resources;
using MoneyFox.ServiceLayer.Utilities;

namespace MoneyFox.Presentation.ViewModels.DesignTime
{
    public class DesignTimeBackupViewModel : IBackupViewModel
    {
        public LocalizedResources Resources { get; } = new LocalizedResources(typeof(Strings), CultureInfo.CurrentUICulture);

        public RelayCommand InitializeCommand { get; }
        public RelayCommand LoginCommand { get; }
        public RelayCommand LogoutCommand { get; }
        public RelayCommand BackupCommand { get; }
        public RelayCommand RestoreCommand { get; }
        public DateTime BackupLastModified { get; }

        public bool IsLoadingBackupAvailability { get; }
        public bool IsLoggedIn { get; } = true;
        public bool BackupAvailable { get; }
    }
}
