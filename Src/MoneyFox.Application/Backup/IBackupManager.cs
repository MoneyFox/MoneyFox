#pragma warning disable S1128 // Unused "using" should be removed
using System;
using System.Threading.Tasks;
using MoneyFox.Domain.Exceptions;

#pragma warning restore S1128 // Unused "using" should be removed

namespace MoneyFox.Application.Backup
{
    /// <summary>
    ///     Defines the interface for a BackupManager who handles the different functions of a backup.
    /// </summary>
    public interface IBackupManager
    {
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
        Task RestoreBackupAsync();

        /// <summary>
        ///     Enqueue a new backup task
        /// </summary>
        /// <exception cref="NetworkConnectionException">Thrown if there is no internet connection.</exception>
        Task EnqueueBackupTaskAsync(int attempts = 0);
    }
}
