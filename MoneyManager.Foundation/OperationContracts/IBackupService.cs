using System;
using System.Threading.Tasks;

namespace MoneyManager.Foundation.OperationContracts {
    public interface IBackupService {
        /// <summary>
        /// Indicates if User is Logged in.
        /// </summary>
        bool IsLoggedIn { get; }
        
        /// <summary>
        /// Shows a login prompt to the user.
        /// </summary>
        void Login();

        /// <summary>
        /// Uploads a copy of the current database.
        /// </summary>
        /// <returns>Returns a TaskCompletionType which indicates if the task was successful or not</returns>
        Task<TaskCompletionType> Upload();

        /// <summary>
        /// Restores a database copy
        /// </summary>
        /// <returns>Returns a TaskCompletionType which indicates if the task was successful or not</returns>
        Task<TaskCompletionType> Restore();

        /// <summary>
        /// Gets the creation date of the current database backup.
        /// </summary>
        /// <returns>Creation date of the current backup</returns>
        Task<DateTime> GetLastCreationDate();
    }
}