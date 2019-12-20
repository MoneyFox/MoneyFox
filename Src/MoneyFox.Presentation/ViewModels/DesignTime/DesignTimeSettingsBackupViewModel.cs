using System;

namespace MoneyFox.Presentation.ViewModels.DesignTime
{
    public class DesignTimeSettingsBackgroundJobViewModel : ISettingsBackgroundJobViewModel
    {
        public bool IsAutoBackupEnabled { get; } = true;
        public int BackupSyncRecurrence { get; } = 3;

        public DateTime LastExecutionSynBackup { get; }
        public DateTime LastExecutionClearPayments { get; }
        public DateTime LastExecutionCreateRecurringPayments { get; }
    }
}
