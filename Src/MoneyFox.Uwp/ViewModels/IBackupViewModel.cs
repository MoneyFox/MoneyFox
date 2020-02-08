using System;
using MoneyFox.Presentation.Commands;

namespace MoneyFox.Presentation.ViewModels
{
    public interface IBackupViewModel
    {
        /// <summary>
        ///     Initialize View Model.
        /// </summary>
        AsyncCommand InitializeCommand { get; }

        /// <summary>
        ///     Makes the first login and sets the setting for the future navigation to this page.
        /// </summary>
        AsyncCommand LoginCommand { get; }

        /// <summary>
        ///     Logs the user out from the backup service.
        /// </summary>
        AsyncCommand LogoutCommand { get; }

        /// <summary>
        ///     Will create a backup of the database and upload it to OneDrive
        /// </summary>
        AsyncCommand BackupCommand { get; }

        /// <summary>
        ///     Will download the database backup from OneDrive and overwrite the     local database with the downloaded.     All
        ///     data models are then reloaded.
        /// </summary>
        AsyncCommand RestoreCommand { get; }

        DateTime BackupLastModified { get; }

        bool IsLoadingBackupAvailability { get; }

        bool IsLoggedIn { get; }

        bool BackupAvailable { get; }
    }
}