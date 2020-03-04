using MoneyFox.Uwp.ViewModels.Settings;

namespace MoneyFox.Uwp.ViewModels.DesignTime
{
    public class DesignTimeSettingsBackgroundJobViewModel : ISettingsBackgroundJobViewModel
    {
        public bool IsAutoBackupEnabled { get; } = true;
        public int BackupSyncRecurrence { get; } = 3;
    }
}
