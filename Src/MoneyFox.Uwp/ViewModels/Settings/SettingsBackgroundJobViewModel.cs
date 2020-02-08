using System;
using GalaSoft.MvvmLight;
using MoneyFox.Application.Common.CloudBackup;
using MoneyFox.Application.Common.Facades;
using MoneyFox.Application.Common.Interfaces;
using MoneyFox.Application.Resources;
using MoneyFox.Presentation.Utilities;

namespace MoneyFox.Presentation.ViewModels.Settings
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
