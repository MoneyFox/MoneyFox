using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using MoneyFox.Foundation.Exceptions;

namespace MoneyFox.BusinessLogic.Interfaces
{
    /// <summary>
    ///     Provides Backup and Restore operations.
    /// </summary>
    public interface ICloudBackupService
    {
        /// <summary>
        ///     Login user.
        /// </summary>
        /// <exception cref="BackupAuthenticationFailedException">Thrown when the user couldn't be logged in.</exception>
        Task Login();

        /// <summary>
        ///     Logout user.
        /// </summary>
        Task Logout();

        /// <summary>
        ///     Uploads a copy of the current database.
        /// </summary>
        /// <param name="dataToUpload">Stream of data to upload.</param>
        /// <returns>Returns a TaskCompletionType which indicates if the task was successful or not</returns>
        /// <exception cref="BackupAuthenticationFailedException">Thrown when the user couldn't be logged in.</exception>
        Task<bool> Upload(Stream dataToUpload);

        /// <summary>
        ///     Restores the file with the passed name
        /// </summary
        /// <param name="backupname">Name of the backup to restore</param>
        /// <param name="dbName">filename in which the database shall be restored.</param>
        /// <returns>TaskCompletionType which indicates if the task was successful or not</returns>
        /// <exception cref="NoBackupFoundException">Thrown when no backup with the right name is found.</exception>
        /// <exception cref="BackupAuthenticationFailedException">Thrown when the user couldn't be logged in.</exception>
        Task<Stream> Restore(string backupname, string dbName);

        /// <summary>
        ///     Gets a list with all the filenames who are available in the backup folder.
        ///     The name of the backupfolder is defined in the Constants.
        /// </summary>
        /// <returns>A list with all filenames.</returns>
        /// <exception cref="BackupAuthenticationFailedException">Thrown when the user couldn't be logged in.</exception>
        Task<List<string>> GetFileNames();

        /// <summary>
        ///     Get's the modification date for the existing backup.
        /// </summary>
        /// <returns>Returns the date of the last backup</returns>
        /// <exception cref="BackupAuthenticationFailedException">Thrown when the user couldn't be logged in.</exception>
        Task<DateTime> GetBackupDate();
    }
}