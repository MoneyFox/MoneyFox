namespace MoneyFox.Domain.Exceptions;

using System.Runtime.Serialization;

public class BackupOperationCanceledException : Exception
{
    public BackupOperationCanceledException() { }

    public BackupOperationCanceledException(string message) : base(message) { }

    public BackupOperationCanceledException(Exception innerException) : base(message: "Backup Operation Canceled!", innerException: innerException) { }
}
