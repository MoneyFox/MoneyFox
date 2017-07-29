using System;
using System.Collections.Generic;
using Windows.ApplicationModel.Background;
using Microsoft.Azure.Mobile.Analytics;
using Microsoft.Toolkit.Uwp;
using MoneyFox.Foundation.Interfaces;

namespace MoneyFox.Windows
{
    /// <inheritdoc />
    public class BackgroundTaskManager : IBackgroundTaskManager
    {
        private readonly ISettingsManager settingsManager;

        /// <summary>
        ///     Constructor
        /// </summary>
        public BackgroundTaskManager(ISettingsManager settingsManager)
        {
            this.settingsManager = settingsManager;
        }

        /// <inheritdoc />
        public void StartBackupSyncTask()
        {
            BackgroundTaskRegistration registered =
                BackgroundTaskHelper.Register(typeof(SyncBackupTask),
                                              new TimeTrigger((uint) (settingsManager.BackupSyncRecurrence * 60),
                                                              true));

            registered.Completed += RegisteredOnCompleted;
        }

        private void RegisteredOnCompleted(BackgroundTaskRegistration sender, BackgroundTaskCompletedEventArgs args)
        {
            var messageDict = new Dictionary<string, string>();
            try
            {
                args.CheckResult();
                messageDict.Add("Successful", "true");
            } catch (Exception ex)
            {
                messageDict.Add("Successful", "false");
                messageDict.Add("Exception", ex.ToString());
            }
            Analytics.TrackEvent("Sync Backup Task finished", messageDict);
        }

        /// <inheritdoc />
        public void StopBackupSyncTask()
        {
            BackgroundTaskHelper.Unregister(typeof(SyncBackupTask));
        }
    }
}