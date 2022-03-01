namespace MoneyFox.Core.Interfaces
{
    using _Pending_.DbBackup;
    using _Pending_.Exceptions;
    using System;
    using System.Threading.Tasks;

    // TODO Segregate interface
    public interface IBackupService
    {
        /// <summary>
        ///     Informations about logged user.
        /// </summary>
        UserAccount UserAccount { get; set; }

        /// <summary>
        ///     Login user.
        /// </summary>
        /// <exception cref="BackupAuthenticationFailedException">Thrown when the user couldn't be logged in.</exception>
        Task LoginAsync();

        /// <summary>
        ///     Logout user.
        /// </summary>
        Task LogoutAsync();

        /// <summary>
        ///     Checks if there are backups to restore.
        /// </summary>
        /// <returns>Backups available or not.</returns>
        /// <exception cref="BackupAuthenticationFailedException">Thrown when the user couldn't be logged in.</exception>
        Task<bool> IsBackupExistingAsync();

        /// <summary>
        ///     Returns the date when the last backup was created.
        /// </summary>
        /// <returns>Creation date of the last backup.</returns>
        /// <exception cref="BackupAuthenticationFailedException">Thrown when the user couldn't be logged in.</exception>
        Task<DateTime> GetBackupDateAsync();

        /// <summary>
        ///     Restores an existing backup.
        /// </summary>
        /// <exception cref="BackupAuthenticationFailedException">Thrown when the user couldn't be logged in.</exception>
        /// <exception cref="NoBackupFoundException">Thrown when no backup with the right name is found.</exception>
        Task RestoreBackupAsync(BackupMode backupMode = BackupMode.Automatic);

        /// <summary>
        ///     Enqueues a new backup task.
        /// </summary>
        /// <exception cref="NetworkConnectionException">Thrown if there is no internet connection.</exception>
        Task UploadBackupAsync(BackupMode backupMode = BackupMode.Automatic);
    }
}