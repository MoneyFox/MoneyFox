namespace MoneyFox.Core.DbBackup
{

    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Threading.Tasks;

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
