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

    int CategorySpreadingNumber { get; set; }

    DateTime LastExecutionTimeStampSyncBackup { get; set; }
}

public class SettingsFacade : ISettingsFacade
{
    private const string AutoUploadBackupKeyName = "AutoUploadBackup";
    private const bool AutoUploadBackupKeyDefault = false;

    private const string BackupLoggedInKeyName = "BackupLoggedIn";
    private const bool BackupLoggedInKeyDefault = false;

    private const string LastExecutionTimeStampSyncBackupKeyName = "LastExecutionTimeStampSyncBackup";
    private const string LastExecutionTimeStampSyncBackupKeyDefault = "";

    private const string DefaultCultureKeyName = "DefaultCulture";

    private const string DatabaseLastUpdateKeyName = "DatabaseLastUpdate";

    private const string IsSetupCompletedKeyName = "IsSetupCompleted";
    private const bool IsSetupCompletedKeyDefault = false;

    private const string CategorySpreadingNumberKeyName = "CategorySpreadingNumber";
    private const int CategorySpreadingNumberDefault = 6;
    private readonly string defaultCultureKeyDefault = CultureInfo.CurrentCulture.Name;

    private readonly ISettingsAdapter settingsAdapter;

    public SettingsFacade(ISettingsAdapter settingsAdapter)
    {
        this.settingsAdapter = settingsAdapter;
    }

    public bool IsBackupAutoUploadEnabled
    {
        get => settingsAdapter.GetValue(key: AutoUploadBackupKeyName, defaultValue: AutoUploadBackupKeyDefault);
        set => settingsAdapter.AddOrUpdate(key: AutoUploadBackupKeyName, value: value);
    }

    public DateTime LastDatabaseUpdate
    {
        get
        {
            var dateString = settingsAdapter.GetValue(key: DatabaseLastUpdateKeyName, defaultValue: DateTime.MinValue.ToString(CultureInfo.InvariantCulture));

            return Convert.ToDateTime(value: dateString, provider: CultureInfo.InvariantCulture);
        }

        set => settingsAdapter.AddOrUpdate(key: DatabaseLastUpdateKeyName, value: value.ToString(CultureInfo.InvariantCulture));
    }

    public bool IsLoggedInToBackupService
    {
        get => settingsAdapter.GetValue(key: BackupLoggedInKeyName, defaultValue: BackupLoggedInKeyDefault);
        set => settingsAdapter.AddOrUpdate(key: BackupLoggedInKeyName, value: value);
    }

    public DateTime LastExecutionTimeStampSyncBackup
    {
        get
            => DateTime.TryParse(
                s: settingsAdapter.GetValue(key: LastExecutionTimeStampSyncBackupKeyName, defaultValue: LastExecutionTimeStampSyncBackupKeyDefault),
                provider: CultureInfo.InvariantCulture,
                styles: DateTimeStyles.None,
                result: out var outValue)
                ? outValue
                : DateTime.MinValue;

        set => settingsAdapter.AddOrUpdate(key: LastExecutionTimeStampSyncBackupKeyName, value: value.ToString(CultureInfo.InvariantCulture));
    }

    public string DefaultCulture
    {
        get => settingsAdapter.GetValue(key: DefaultCultureKeyName, defaultValue: defaultCultureKeyDefault);
        set => settingsAdapter.AddOrUpdate(key: DefaultCultureKeyName, value: value);
    }

    public bool IsSetupCompleted
    {
        get => settingsAdapter.GetValue(key: IsSetupCompletedKeyName, defaultValue: IsSetupCompletedKeyDefault);
        set => settingsAdapter.AddOrUpdate(key: IsSetupCompletedKeyName, value: value);
    }

    public int CategorySpreadingNumber
    {
        get => settingsAdapter.GetValue(key: CategorySpreadingNumberKeyName, defaultValue: CategorySpreadingNumberDefault);
        set => settingsAdapter.AddOrUpdate(key: CategorySpreadingNumberKeyName, value: value);
    }
}
