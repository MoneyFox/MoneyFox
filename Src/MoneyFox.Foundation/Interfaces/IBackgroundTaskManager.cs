namespace MoneyFox.Foundation.Interfaces
{
    public interface IBackgroundTaskManager
    {
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