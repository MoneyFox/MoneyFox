namespace MoneyFox.Core.Features.DbBackup;

using System;
using System.Threading.Tasks;

public interface IBackupService
{
    Task LoginAsync();

    Task LogoutAsync();

    Task<bool> IsBackupExistingAsync();

    Task<DateTime> GetBackupDateAsync();

    Task RestoreBackupAsync(BackupMode backupMode = BackupMode.Automatic);
}
