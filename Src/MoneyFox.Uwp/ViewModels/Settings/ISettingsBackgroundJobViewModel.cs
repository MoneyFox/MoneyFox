using System;

namespace MoneyFox.Uwp.ViewModels.Settings
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
}
