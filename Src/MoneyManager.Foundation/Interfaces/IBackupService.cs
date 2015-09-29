using System.Threading.Tasks;

namespace MoneyManager.Foundation.Interfaces
{
    public interface IBackupService
    {
        /// <summary>
        ///     Indicates if User is Logged in.
        /// </summary>
        bool IsLoggedIn { get; }

        /// <summary>
        ///     Shows a login prompt to the user.
        /// </summary>
        Task Login();

        /// <summary>
        ///     Uploads a copy of the current database.
        /// </summary>
        /// <returns>Returns a TaskCompletionType which indicates if the task was successful or not</returns>
        Task<TaskCompletionType> Upload();

        /// <summary>
        ///     Restores a database copy
        /// </summary>
        /// <returns>Returns a TaskCompletionType which indicates if the task was successful or not</returns>
        Task<TaskCompletionType> Restore();
    }
}