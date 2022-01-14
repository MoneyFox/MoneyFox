using CommunityToolkit.Mvvm.Input;
using System;

namespace MoneyFox.Uwp.ViewModels.DataBackup
{
    public class DesignTimeBackupViewModel : IBackupViewModel
    {
        public AsyncRelayCommand InitializeCommand { get; } = null!;

        public AsyncRelayCommand LoginCommand { get; } = null!;

        public AsyncRelayCommand LogoutCommand { get; } = null!;

        public AsyncRelayCommand BackupCommand { get; } = null!;

        public AsyncRelayCommand RestoreCommand { get; } = null!;

        public DateTime BackupLastModified { get; }

        public bool IsLoadingBackupAvailability { get; }

        public bool IsLoggedIn { get; } = true;

        public bool BackupAvailable { get; }

        public bool IsAutoBackupEnabled { get; }
    }
}