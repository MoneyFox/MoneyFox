using MoneyFox.Ui.Shared.Commands;
using System;

namespace MoneyFox.Presentation.ViewModels.DesignTime
{
    public class DesignTimeBackupViewModel : IBackupViewModel
    {
        public AsyncCommand InitializeCommand { get; }

        public AsyncCommand LoginCommand { get; }

        public AsyncCommand LogoutCommand { get; }

        public AsyncCommand BackupCommand { get; }

        public AsyncCommand RestoreCommand { get; }

        public DateTime BackupLastModified { get; }

        public bool IsLoadingBackupAvailability { get; }

        public bool IsLoggedIn { get; } = true;

        public bool BackupAvailable { get; }
    }
}
