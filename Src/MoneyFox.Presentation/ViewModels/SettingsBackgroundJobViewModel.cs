using GalaSoft.MvvmLight;
using MoneyFox.Application.Common.Facades;
using System;

namespace MoneyFox.Presentation.ViewModels
{
    public interface ISettingsBackgroundJobViewModel
    {
        /// <summary>
        ///     Indicates if the autobackup is enabled or disabled.
        /// </summary>
        bool IsAutoBackupEnabled { get; }

        DateTime LastExecutionSynBackup { get; }
        DateTime LastExecutionClearPayments { get; }
        DateTime LastExecutionCreateRecurringPayments { get; }
    }

    /// <inheritdoc cref="ISettingsBackgroundJobViewModel" />
    /// />
    public class SettingsBackgroundJobViewModel : ViewModelBase, ISettingsBackgroundJobViewModel
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

        public DateTime LastExecutionSynBackup => settingsFacade.LastExecutionTimeStampSyncBackup;
        public DateTime LastExecutionClearPayments => settingsFacade.LastExecutionTimeStampClearPayments;
        public DateTime LastExecutionCreateRecurringPayments => settingsFacade.LastExecutionTimeStampRecurringPayments;
    }
}
