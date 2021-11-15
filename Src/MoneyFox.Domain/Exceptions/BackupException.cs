﻿using System;
using System.Runtime.Serialization;

namespace MoneyFox.Domain.Exceptions
{
    /// <summary>
    ///     This Exception is thrown when something went wrong during backup the database.
    /// </summary>
    [Serializable]
    public class BackupException : Exception
    {
        /// <summary>
        ///     Creates an Backup Exception
        /// </summary>
        public BackupException()
        {
        }

        /// <summary>
        ///     Creates an Backup Exception
        /// </summary>
        /// <param name="message">Exception message to show to the user.</param>
        public BackupException(string message) : base(message)
        {
        }

        /// <summary>
        ///     Creates an Backup Exception
        /// </summary>
        /// <param name="message">Exception message to show to the user.</param>
        /// <param name="exception">Inner Exception of the backup exception.</param>
        public BackupException(string message, Exception exception) : base(message, exception)
        {
        }

        protected BackupException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}