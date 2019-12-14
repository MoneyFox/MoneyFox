using MoneyFox.Application.Common.Facades;
using System;

namespace MoneyFox.Presentation.ViewModels
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

    /// <inheritdoc cref="ISettingsBackgroundJobViewModel" />
    /// />
    public class SettingsBackgroundJobViewModel : BaseViewModel, ISettingsBackgroundJobViewModel
    {
        private readonly ISettingsFacade settingsFacade;

        /// <summary>
        ///     Constructor
        /// </summary>
        public SettingsBackgroundJobViewModel(ISettingsFacade settingsFacade)
        {
            this.settingsFacade = settingsFacade;
        }

        /// <inheritdoc />
        public bool IsAutoBackupEnabled
        {
            get => settingsFacade.IsBackupAutouploadEnabled;
            set
            {
                if (settingsFacade.IsBackupAutouploadEnabled == value) return;
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
                if (settingsFacade.BackupSyncRecurrence == value) return;
                settingsFacade.BackupSyncRecurrence = value < 1 ? 1 : value;
                RaisePropertyChanged();
            }
        }

        public DateTime LastExecutionSynBackup => settingsFacade.LastExecutionTimeStampSyncBackup;
        public DateTime LastExecutionClearPayments => settingsFacade.LastExecutionTimeStampClearPayments;
        public DateTime LastExecutionCreateRecurringPayments => settingsFacade.LastExecutionTimeStampRecurringPayments;
    }
}
