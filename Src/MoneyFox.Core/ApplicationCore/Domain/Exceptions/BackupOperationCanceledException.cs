namespace MoneyFox.Core.ApplicationCore.Domain.Exceptions;

using System;
using System.Runtime.Serialization;

[Serializable]
public class BackupOperationCanceledException : Exception
{
    public BackupOperationCanceledException() { }

    public BackupOperationCanceledException(string message) : base(message) { }

    public BackupOperationCanceledException(Exception innerException) : base(message: "Backup Operation Canceled!", innerException: innerException) { }

    public BackupOperationCanceledException(string message, Exception innerException) : base(message: message, innerException: innerException) { }

    protected BackupOperationCanceledException(SerializationInfo info, StreamingContext context) : base(info: info, context: context) { }
}
