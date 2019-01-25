using System;
using MoneyFox.Foundation.Interfaces;
using MoneyFox.ServiceLayer.Facades;
using MvvmCross.Logging;
using MvvmCross.Navigation;

namespace MoneyFox.ServiceLayer.ViewModels
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

        DateTime LastExecutionSynBackup { get; }
        DateTime LastExecutionClearPayments { get; }
        DateTime LastExecutionCreateRecurringPayments { get; }
    }
    
    /// <inheritdoc cref="ISettingsBackgroundJobViewModel"/>/>
    public class SettingsBackgroundJobViewModel : BaseNavigationViewModel, ISettingsBackgroundJobViewModel
    {
        private readonly ISettingsFacade settingsFacade;
        private readonly IBackgroundTaskManager backgroundTaskManager;

        /// <summary>
        ///     Constructor
        /// </summary>
        public SettingsBackgroundJobViewModel(ISettingsFacade settingsFacade,
                                              IBackgroundTaskManager backgroundTaskManager,
                                              IMvxLogProvider logProvider,
                                              IMvxNavigationService navigationService) : base(logProvider, navigationService)
        {
            this.settingsFacade = settingsFacade;
            this.backgroundTaskManager = backgroundTaskManager;
        }

        /// <inheritdoc />
        public bool IsAutoBackupEnabled
        {
            get => settingsFacade.IsBackupAutouploadEnabled;
            set
            {
                if (settingsFacade.IsBackupAutouploadEnabled == value) return;

                if (settingsFacade.IsBackupAutouploadEnabled)
                {
                    backgroundTaskManager.StopBackupSyncTask();
                } 
                else
                {
                    backgroundTaskManager.StartBackupSyncTask(settingsFacade.BackupSyncRecurrence * 60);
                }
                settingsFacade.IsBackupAutouploadEnabled = value;
                RaisePropertyChanged();
            }
        }

        /// <inheritdoc />
        public int BackupSyncRecurrence
        {
            get => settingsFacade.BackupSyncRecurrence;
            set
            {
                if(settingsFacade.BackupSyncRecurrence == value) return;
                settingsFacade.BackupSyncRecurrence = value < 1 ? 1 : value;
                backgroundTaskManager.StopBackupSyncTask();
                backgroundTaskManager.StartBackupSyncTask(settingsFacade.BackupSyncRecurrence * 60);
                RaisePropertyChanged();
            }
        }

        public DateTime LastExecutionSynBackup => settingsFacade.LastExecutionTimeStampSyncBackup;
        public DateTime LastExecutionClearPayments => settingsFacade.LastExecutionTimeStampClearPayments;
        public DateTime LastExecutionCreateRecurringPayments => settingsFacade.LastExecutionTimeStampRecurringPayments;
    }
}