using GalaSoft.MvvmLight.Command;
using System;

namespace MoneyFox.Ui.Shared.ViewModels.Backup
{
    public interface IBackupViewModel
    {
        /// <summary>
        /// Initialize View Model.
        /// </summary>
        RelayCommand InitializeCommand { get; }

        /// <summary>
        /// Makes the first login and sets the setting for the future navigation to this page.
        /// </summary>
        RelayCommand LoginCommand { get; }

        /// <summary>
        /// Logs the user out from the backup service.
        /// </summary>
        RelayCommand LogoutCommand { get; }

        /// <summary>
        /// Will create a backup of the database and upload it to OneDrive
        /// </summary>
        RelayCommand BackupCommand { get; }

        /// <summary>
        /// Will download the database backup from OneDrive and overwrite the     local database with the downloaded.     All
        ///  data models are then reloaded.
        /// </summary>
        RelayCommand RestoreCommand { get; }

        DateTime BackupLastModified { get; }

        bool IsLoadingBackupAvailability { get; }

        bool IsLoggedIn { get; }

        bool BackupAvailable { get; }

        /// <summary>
        /// Indicates if the autobackup is enabled or disabled.
        /// </summary>
        bool IsAutoBackupEnabled { get; }

    }
}
