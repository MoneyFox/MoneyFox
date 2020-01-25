using System;
using System.Runtime.Serialization;

namespace MoneyFox.Domain.Exceptions
{
    [Serializable]
    public class BackupOperationCanceledException : Exception
    {
        public BackupOperationCanceledException()
        { }

        public BackupOperationCanceledException(string message) : base(message)
        { }

        public BackupOperationCanceledException(Exception innerException) : base("Backup Operation Canceled!", innerException)
        { }

        public BackupOperationCanceledException(string message, Exception innerException) : base(message, innerException)
        { }

        protected BackupOperationCanceledException(SerializationInfo info, StreamingContext context) : base(info, context)
        { }
    }
}
