namespace MoneyFox.Core.Common.Exceptions
{
    using System;
    using System.Runtime.Serialization;

    /// <summary>
    ///     This Exception is thrown when the user couldn't authenticate on the backup service.
    /// </summary>
    [Serializable]
    public class BackupAuthenticationFailedException : Exception
    {
        public BackupAuthenticationFailedException()
        {
        }

        public BackupAuthenticationFailedException(string message) : base(message)
        {
        }

        public BackupAuthenticationFailedException(Exception exception) : base(
            "Backup Authentication Failed",
            exception)
        {
        }

        public BackupAuthenticationFailedException(string message, Exception exception) : base(message, exception)
        {
        }

        protected BackupAuthenticationFailedException(SerializationInfo info, StreamingContext context) : base(
            info,
            context)
        {
        }
    }
}