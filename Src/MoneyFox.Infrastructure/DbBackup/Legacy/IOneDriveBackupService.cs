namespace MoneyFox.Infrastructure.DbBackup.Legacy
{

    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Threading.Tasks;
    using MoneyFox.Core.ApplicationCore.UseCases.DbBackup;

    public interface IOneDriveBackupService
    {
        Task LoginAsync();

        Task LogoutAsync();

        Task<UserAccount> GetUserAccountAsync();

        Task<bool> UploadAsync(Stream dataToUpload);

        Task<Stream> RestoreAsync();

        Task<List<string>> GetFileNamesAsync();

        Task<DateTime> GetBackupDateAsync();
    }

}
