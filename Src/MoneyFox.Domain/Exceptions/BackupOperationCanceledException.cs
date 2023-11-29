namespace MoneyFox.Domain.Exceptions;

public class BackupOperationCanceledException : Exception
{
    public BackupOperationCanceledException() { }

    public BackupOperationCanceledException(string message) : base(message) { }

    public BackupOperationCanceledException(Exception innerException) : base(message: "Backup Operation Canceled!", innerException: innerException) { }
}
