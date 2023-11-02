namespace MoneyFox.Core.Features;

using System.IO;

public interface ISqliteBackupService
{
    (string backupName, FileStream backupAsStream) CreateBackup();
}
