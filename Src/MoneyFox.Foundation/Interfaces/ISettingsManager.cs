using System;

namespace MoneyFox.Foundation.Interfaces
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
        //get and set for defualt language
        bool IsDefaultLanguageChosen{ get; set; }

    }
}