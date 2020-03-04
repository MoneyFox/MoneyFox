using System;
using System.Runtime.Serialization;

namespace MoneyFox.Domain.Exceptions
{
    /// <summary>
    /// This Exception is thrown when on a backup restore no backup with the right name was found.
    /// </summary>
    [Serializable]
    public class NoBackupFoundException : Exception
    {
        /// <summary>
        /// Creates an NoBackupFound Exception
        /// </summary>
        public NoBackupFoundException()
        {
        }

        /// <summary>
        /// Creates an NoBackupFound Exception
        /// </summary>
        /// <param name="message">Exception message to show to the user.</param>
        public NoBackupFoundException(string message) : base(message)
        {
        }

        /// <summary>
        /// Creates an NoBackupFound Exception
        /// </summary>
        /// <param name="message">Exception message to show to the user.</param>
        /// <param name="exception">Inner Exception of the backup exception.</param>
        public NoBackupFoundException(string message, Exception exception) : base(message, exception)
        {
        }

        protected NoBackupFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
