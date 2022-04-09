namespace MoneyFox.Win.ViewModels.DataBackup;

using System;
using CommunityToolkit.Mvvm.Input;

public interface IBackupViewModel
{
    /// <summary>
    ///     Initialize View Model.
    /// </summary>
    AsyncRelayCommand InitializeCommand { get; }

    /// <summary>
    ///     Makes the first login and sets the setting for the future navigation to this page.
    /// </summary>
    AsyncRelayCommand LoginCommand { get; }

    /// <summary>
    ///     Logs the user out from the backup service.
    /// </summary>
    AsyncRelayCommand LogoutCommand { get; }

    /// <summary>
    ///     Will create a backup of the database and upload it to OneDrive
    /// </summary>
    AsyncRelayCommand BackupCommand { get; }

    /// <summary>
    ///     Will download the database backup from OneDrive and overwrite the     local database with the downloaded.     All
    ///     data models are then reloaded.
    /// </summary>
    AsyncRelayCommand RestoreCommand { get; }

    DateTime BackupLastModified { get; }

    bool IsLoadingBackupAvailability { get; }

    bool IsLoggedIn { get; }

    bool BackupAvailable { get; }

    /// <summary>
    ///     Indicates if the autobackup is enabled or disabled.
    /// </summary>
    bool IsAutoBackupEnabled { get; }
}
