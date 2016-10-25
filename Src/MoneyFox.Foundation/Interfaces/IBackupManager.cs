using System;
using System.Threading.Tasks;

namespace MoneyFox.Foundation.Interfaces
{
    /// <summary>
    ///     Defines the interface for a BackupManager who handles the different functions of a backup.
    /// </summary>
    public interface IBackupManager
    {
        /// <summary>
        ///     Login user.
        /// </summary>
        Task Login();        
        
        /// <summary>
        ///     Logout user.
        /// </summary>
        Task Logout();

        /// <summary>
        ///     Checks if there are backups to restore.
        /// </summary>
        /// <returns>Backups available or not.</returns>
        Task<bool> IsBackupExisting();

        /// <summary>
        ///     Returns the date when the last backup was created.
        /// </summary>
        /// <returns>Creation date of the last backup.</returns>
        Task<DateTime> GetBackupDate();

        /// <summary>
        ///     Creates a new backup.
        /// </summary>
        Task<bool> CreateNewBackup();

        /// <summary>
        ///     Restores an existing backup.
        /// </summary>
        Task RestoreBackup();

        /// <summary>
        ///     Enqueues a new backup task
        /// </summary>
        Task EnqueueBackupTask(int attempts = 5);
    }
}