using System;

namespace MoneyFox.Shared.Interfaces
{
    public interface ISettingsManager
    {
        int DefaultAccount { get; set; }
        bool ShowCashFlowOnMainTile { get; set; }
        bool IsBackupAutouploadEnabled { get; set; }
        string SessionTimestamp { get; set; }
        bool PasswordRequired { get; set; }
        DateTime LastDatabaseUpdate { get; set; }
        bool IsDarkThemeSelected { get; set; }
        bool IsLoggedInToBackupService { get; set; }
    }
}