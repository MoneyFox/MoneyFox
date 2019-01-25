using System;
using System.Globalization;
using MoneyFox.Foundation.Resources;
using MoneyFox.ServiceLayer.Utilities;

namespace MoneyFox.ServiceLayer.ViewModels.DesignTime
{
    public class DesignTimeSettingsBackgroundJobViewModel : ISettingsBackgroundJobViewModel
    {
        public DesignTimeSettingsBackgroundJobViewModel()
        {
            Resources = new LocalizedResources(typeof(Strings), CultureInfo.CurrentUICulture);
        }

        /// <inheritdoc />
        public LocalizedResources Resources { get; }

        public bool IsAutoBackupEnabled { get; } = true;
        public int BackupSyncRecurrence { get; } = 3;

        public DateTime LastExecutionSynBackup { get; }
        public DateTime LastExecutionClearPayments { get; }
        public DateTime LastExecutionCreateRecurringPayments { get; }
    }
}
