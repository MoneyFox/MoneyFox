using MoneyFox.Foundation.Interfaces;
using MvvmCross.Logging;
using MvvmCross.Navigation;

namespace MoneyFox.Business.ViewModels
{
    public interface ISettingsBackgroundJobViewModel : IBaseViewModel
    {        
        /// <summary>
        ///     Indicates if the autobackup is enabled or disabled.
        /// </summary>
        bool IsAutoBackupEnabled { get; }

        /// <summary>
        ///     Amount of hours to sync the backup.
        /// </summary>
        int BackupSyncRecurrence { get; }
    }
    
    /// <inheritdoc cref="ISettingsBackgroundJobViewModel"/>/>
    public class SettingsBackgroundJobViewModel : BaseViewModel, ISettingsBackgroundJobViewModel
    {
        private readonly ISettingsManager settingsManager;
        private readonly IBackgroundTaskManager backgroundTaskManager;

        /// <summary>
        ///     Constructor
        /// </summary>
        public SettingsBackgroundJobViewModel(ISettingsManager settingsManager,
                                              IBackgroundTaskManager backgroundTaskManager,
                                              IMvxLogProvider logProvider,
                                              IMvxNavigationService navigationService) : base(logProvider, navigationService)
        {
            this.settingsManager = settingsManager;
            this.backgroundTaskManager = backgroundTaskManager;
        }

        /// <inheritdoc />
        public bool IsAutoBackupEnabled
        {
            get => settingsManager.IsBackupAutouploadEnabled;
            set
            {
                if (settingsManager.IsBackupAutouploadEnabled == value) return;

                if (settingsManager.IsBackupAutouploadEnabled)
                {
                    backgroundTaskManager.StopBackupSyncTask();
                } 
                else
                {
                    backgroundTaskManager.StartBackupSyncTask(settingsManager.BackupSyncRecurrence * 60);
                }
                settingsManager.IsBackupAutouploadEnabled = value;
                RaisePropertyChanged();
            }
        }

        /// <inheritdoc />
        public int BackupSyncRecurrence
        {
            get => settingsManager.BackupSyncRecurrence;
            set
            {
                if(settingsManager.BackupSyncRecurrence == value) return;
                settingsManager.BackupSyncRecurrence = value < 1 ? 1 : value;
                backgroundTaskManager.StopBackupSyncTask();
                backgroundTaskManager.StartBackupSyncTask(settingsManager.BackupSyncRecurrence * 60);
                RaisePropertyChanged();
            }
        }
    }
}