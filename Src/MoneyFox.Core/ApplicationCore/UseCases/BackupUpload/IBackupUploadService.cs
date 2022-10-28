namespace MoneyFox.Core.ApplicationCore.UseCases.BackupUpload;

using System;
using System.IO;
using System.Threading.Tasks;

public interface IBackupUploadService
{
    Task<DateTime> GetBackupDateAsync();

    Task UploadAsync(string backupName, Stream dataToUpload);

    Task<int> GetBackupCount();

    Task DeleteOldest();
}
