using System;
using GalaSoft.MvvmLight;
using MoneyFox.Application.Common.Facades;

namespace MoneyFox.Uwp.ViewModels.Settings
{
    /// <inheritdoc cref="ISettingsBackgroundJobViewModel" />
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
