namespace MoneyFox.Infrastructure.DbBackup.Legacy;

using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

public interface IOneDriveBackupService
{
    Task LoginAsync();

    Task LogoutAsync();

    Task<Stream> RestoreAsync();

    Task<List<string>> GetFileNamesAsync();

    Task<DateTime> GetBackupDateAsync();
}
