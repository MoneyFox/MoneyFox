using System.Threading.Tasks;

namespace MoneyFox.Core.Interfaces
{
    public interface IBackgroundTaskService
    {
        /// <summary>
        ///     Will register a background task asynchronously
        /// </summary>
        /// <returns>Task for the waiter.</returns>
        Task RegisterTasksAsync();
    }
}