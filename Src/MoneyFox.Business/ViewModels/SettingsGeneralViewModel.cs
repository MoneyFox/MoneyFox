using MoneyFox.Foundation.Interfaces;
using MvvmCross.Localization;

namespace MoneyFox.Business.ViewModels
{
    public class SettingsGeneralViewModel : BaseViewModel
    {
        private readonly ISettingsManager settingsManager;
        private readonly IBackgroundTaskManager backgroundTaskManager;

        public SettingsGeneralViewModel(ISettingsManager settingsManager, IBackgroundTaskManager backgroundTaskManager)
        {
            this.settingsManager = settingsManager;
            this.backgroundTaskManager = backgroundTaskManager;
        }

        /// <summary>
        ///     Provides an TextSource for the translation binding on this page.
        /// </summary>
        public IMvxLanguageBinder TextSource => new MvxLanguageBinder("", GetType().Name);

        /// <summary>
        ///     Indicates if the autobackup is enabled or disabled.
        /// </summary>
        public bool IsAutoBackupEnabled
        {
            get { return settingsManager.IsBackupAutouploadEnabled; }
            set
            {
                settingsManager.IsBackupAutouploadEnabled = value;
                backgroundTaskManager.StartBackupSyncTask();
                RaisePropertyChanged();
            }
        }

        /// <summary>
        ///     Amount of hours to sync the backup.
        /// </summary>
        public int BackupSyncRecurrence
        {
            get { return settingsManager.BackupSyncRecurrence; }
            set
            {
                settingsManager.BackupSyncRecurrence = value;
                backgroundTaskManager.StartBackupSyncTask();
                RaisePropertyChanged();
            }
        }
    }
}