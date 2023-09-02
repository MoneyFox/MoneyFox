namespace MoneyFox.Core.Common.Settings;

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

    string DefaultCurrency { get; set; }

    DateTime LastExecutionTimeStampSyncBackup { get; set; }
    bool RecurringTransactionMigrated { get; set; }
    bool RecurringTransactionMigrated2 { get; set; }
    bool RecurringTransactionMigrated3 { get; set; }
}

public class SettingsFacade : ISettingsFacade
{
    private readonly string defaultCultureKeyDefault = CultureInfo.CurrentCulture.Name;

    private readonly ISettingsAdapter settingsAdapter;

    public SettingsFacade(ISettingsAdapter settingsAdapter)
    {
        this.settingsAdapter = settingsAdapter;
    }

    public bool IsBackupAutoUploadEnabled
    {
        get => settingsAdapter.GetValue(key: SettingConstants.AUTO_UPLOAD_BACKUP_KEY_NAME, defaultValue: false);
        set => settingsAdapter.AddOrUpdate(key: SettingConstants.AUTO_UPLOAD_BACKUP_KEY_NAME, value: value);
    }

    public DateTime LastDatabaseUpdate
    {
        get
        {
            var dateString = settingsAdapter.GetValue(
                key: SettingConstants.DATABASE_LAST_UPDATE_KEY_NAME,
                defaultValue: DateTime.MinValue.ToString(CultureInfo.InvariantCulture));

            return Convert.ToDateTime(value: dateString, provider: CultureInfo.InvariantCulture);
        }

        set => settingsAdapter.AddOrUpdate(key: SettingConstants.DATABASE_LAST_UPDATE_KEY_NAME, value: value.ToString(CultureInfo.InvariantCulture));
    }

    public bool IsLoggedInToBackupService
    {
        get => settingsAdapter.GetValue(key: SettingConstants.BACKUP_LOGGED_IN_KEY_NAME, defaultValue: false);
        set => settingsAdapter.AddOrUpdate(key: SettingConstants.BACKUP_LOGGED_IN_KEY_NAME, value: value);
    }

    public DateTime LastExecutionTimeStampSyncBackup
    {
        get
            => DateTime.TryParse(
                s: settingsAdapter.GetValue(key: SettingConstants.LAST_EXECUTION_TIME_STAMP_SYNC_BACKUP_KEY_NAME, defaultValue: string.Empty),
                provider: CultureInfo.InvariantCulture,
                styles: DateTimeStyles.None,
                result: out var outValue)
                ? outValue
                : DateTime.MinValue;

        set
            => settingsAdapter.AddOrUpdate(
                key: SettingConstants.LAST_EXECUTION_TIME_STAMP_SYNC_BACKUP_KEY_NAME,
                value: value.ToString(CultureInfo.InvariantCulture));
    }

    public string DefaultCulture
    {
        get => settingsAdapter.GetValue(key: SettingConstants.DEFAULT_CULTURE_KEY_NAME, defaultValue: defaultCultureKeyDefault);
        set => settingsAdapter.AddOrUpdate(key: SettingConstants.DEFAULT_CULTURE_KEY_NAME, value: value);
    }

    public bool IsSetupCompleted
    {
        get => settingsAdapter.GetValue(key: SettingConstants.IS_SETUP_COMPLETED_KEY_NAME, defaultValue: false);
        set => settingsAdapter.AddOrUpdate(key: SettingConstants.IS_SETUP_COMPLETED_KEY_NAME, value: value);
    }

    public string DefaultCurrency
    {
        get => settingsAdapter.GetValue(key: SettingConstants.DEFAULT_CURRENCY_KEY_NAME, defaultValue: string.Empty);
        set => settingsAdapter.AddOrUpdate(key: SettingConstants.DEFAULT_CURRENCY_KEY_NAME, value: value);
    }

    public bool RecurringTransactionMigrated
    {
        get => settingsAdapter.GetValue(key: SettingConstants.RECURRING_TRANSACTION_MIGRATED, defaultValue: false);
        set => settingsAdapter.AddOrUpdate(key: SettingConstants.RECURRING_TRANSACTION_MIGRATED, value: value);
    }

    public bool RecurringTransactionMigrated2
    {
        get => settingsAdapter.GetValue(key: SettingConstants.RECURRING_TRANSACTION_MIGRATED2, defaultValue: false);
        set => settingsAdapter.AddOrUpdate(key: SettingConstants.RECURRING_TRANSACTION_MIGRATED2, value: value);
    }

    public bool RecurringTransactionMigrated3
    {
        get => settingsAdapter.GetValue(key: SettingConstants.RECURRING_TRANSACTION_MIGRATED3, defaultValue: false);
        set => settingsAdapter.AddOrUpdate(key: SettingConstants.RECURRING_TRANSACTION_MIGRATED3, value: value);
    }
}
