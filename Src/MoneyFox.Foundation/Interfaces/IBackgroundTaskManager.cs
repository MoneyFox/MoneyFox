namespace MoneyFox.Foundation.Interfaces
{
    /// <summary>
    ///     Handles the Starting and stopping of background tasks.
    /// </summary>
    public interface IBackgroundTaskManager
    {
        /// <summary>
        ///     Stops all Background tasks.
        /// </summary>
        void StopBackgroundTasks();

        /// <summary>
        ///     Starts all Background tasks.
        /// </summary>
        void StartBackgroundTasks();

        /// <summary>
        ///     Starts the backup sync task.
        /// </summary>
        void StartBackupSyncTask();

        /// <summary>
        ///     Stops the background sync task.
        /// </summary>
        void StopBackupSyncTask();
    }
}