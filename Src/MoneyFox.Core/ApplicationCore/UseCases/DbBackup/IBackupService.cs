namespace MoneyFox.Core.ApplicationCore.UseCases.DbBackup
{

    using System;
    using System.Threading.Tasks;

    // TODO Segregate interface
    public interface IBackupService
    {
        Task LoginAsync();

        Task LogoutAsync();

        Task<UserAccountDto> GetUserAccount();

        Task<bool> IsBackupExistingAsync();

        Task<DateTime> GetBackupDateAsync();

        Task RestoreBackupAsync(BackupMode backupMode = BackupMode.Automatic);
    }

}
