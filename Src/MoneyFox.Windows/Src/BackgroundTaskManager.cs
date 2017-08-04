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
                BackgroundTaskHelper.Register("SyncBackgroundTask", new TimeTrigger((uint) (settingsManager.BackupSyncRecurrence * 60), false));
        }

        /// <inheritdoc />
        public void StopBackupSyncTask()
        {
            BackgroundTaskHelper.Unregister(typeof(SyncBackupTask));
        }
    }
}