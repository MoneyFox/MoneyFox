using System.Threading.Tasks;

namespace MoneyFox.Shared.Interfaces {
    public interface IBackgroundTaskService {
        /// <summary>
        ///     Will register a background task asynchronously
        /// </summary>
        /// <returns>Task for the waiter.</returns>
        Task RegisterTasksAsync();
    }
}