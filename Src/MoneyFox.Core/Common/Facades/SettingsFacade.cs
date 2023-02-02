namespace MoneyFox.Core.Common.Facades;

using System;
using System.Globalization;
using Core.Interfaces;

public interface ISettingsFacade
{
    bool IsBackupAutoUploadEnabled { get; set; }

    DateTime LastDatabaseUpdate { get; set; }

    bool IsLoggedInToBackupService { get; set; }

    string DefaultCulture { get; set; }

    bool IsSetupCompleted { get; set; }

    bool IsCategoryCleanupExecuted { get; set; }

    DateTime LastExecutionTimeStampSyncBackup { get; set; }
}

public class SettingsFacade : ISettingsFacade
{
    private readonly string defaultCultureKeyDefault = CultureInfo.CurrentCulture.Name;
    private const string DEFAULT_CULTURE_KEY_NAME = "DefaultCulture";

    private const string AUTO_UPLOAD_BACKUP_KEY_NAME = "AutoUploadBackup";
    private const string BACKUP_LOGGED_IN_KEY_NAME = "BackupLoggedIn";
    private const string LAST_EXECUTION_TIME_STAMP_SYNC_BACKUP_KEY_NAME = "LastExecutionTimeStampSyncBackup";
    private const string DATABASE_LAST_UPDATE_KEY_NAME = "DatabaseLastUpdate";
    private const string IS_SETUP_COMPLETED_KEY_NAME = "IsSetupCompleted";
    private const string IS_CATEGORY_CLEANUP_EXECUTED_KEY_NAME = "CategoryCleanupExecuted";

    private readonly ISettingsAdapter settingsAdapter;

    public SettingsFacade(ISettingsAdapter settingsAdapter)
    {
        this.settingsAdapter = settingsAdapter;
    }

    public bool IsBackupAutoUploadEnabled
    {
        get => settingsAdapter.GetValue(key: AUTO_UPLOAD_BACKUP_KEY_NAME, defaultValue: false);
        set => settingsAdapter.AddOrUpdate(key: AUTO_UPLOAD_BACKUP_KEY_NAME, value: value);
    }

    public DateTime LastDatabaseUpdate
    {
        get
        {
            var dateString = settingsAdapter.GetValue(key: DATABASE_LAST_UPDATE_KEY_NAME, defaultValue: DateTime.MinValue.ToString(CultureInfo.InvariantCulture));

            return Convert.ToDateTime(value: dateString, provider: CultureInfo.InvariantCulture);
        }

        set => settingsAdapter.AddOrUpdate(key: DATABASE_LAST_UPDATE_KEY_NAME, value: value.ToString(CultureInfo.InvariantCulture));
    }

    public bool IsLoggedInToBackupService
    {
        get => settingsAdapter.GetValue(key: BACKUP_LOGGED_IN_KEY_NAME, defaultValue: false);
        set => settingsAdapter.AddOrUpdate(key: BACKUP_LOGGED_IN_KEY_NAME, value: value);
    }

    public DateTime LastExecutionTimeStampSyncBackup
    {
        get
            => DateTime.TryParse(
                s: settingsAdapter.GetValue(key: LAST_EXECUTION_TIME_STAMP_SYNC_BACKUP_KEY_NAME, defaultValue: string.Empty),
                provider: CultureInfo.InvariantCulture,
                styles: DateTimeStyles.None,
                result: out var outValue)
                ? outValue
                : DateTime.MinValue;

        set => settingsAdapter.AddOrUpdate(key: LAST_EXECUTION_TIME_STAMP_SYNC_BACKUP_KEY_NAME, value: value.ToString(CultureInfo.InvariantCulture));
    }

    public string DefaultCulture
    {
        get => settingsAdapter.GetValue(key: DEFAULT_CULTURE_KEY_NAME, defaultValue: defaultCultureKeyDefault);
        set => settingsAdapter.AddOrUpdate(key: DEFAULT_CULTURE_KEY_NAME, value: value);
    }

    public bool IsSetupCompleted
    {
        get => settingsAdapter.GetValue(key: IS_SETUP_COMPLETED_KEY_NAME, defaultValue: false);
        set => settingsAdapter.AddOrUpdate(key: IS_SETUP_COMPLETED_KEY_NAME, value: value);
    }

    public bool IsCategoryCleanupExecuted
    {
        get => settingsAdapter.GetValue(key: IS_CATEGORY_CLEANUP_EXECUTED_KEY_NAME, defaultValue: false);
        set => settingsAdapter.AddOrUpdate(key: IS_CATEGORY_CLEANUP_EXECUTED_KEY_NAME, value: value);
    }
}
