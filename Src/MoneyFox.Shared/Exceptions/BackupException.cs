using System;

namespace MoneyFox.Shared.Exceptions {
    public class BackupException : Exception {
        /// <summary>
        ///     Creates an Backup Exception
        /// </summary>
        public BackupException() {
        }

        /// <summary>
        ///     Creates an Backup Exception
        /// </summary>
        /// <param name="message">Exception message to show to the user.</param>
        public BackupException(string message)
            : base(message) {
        }

        /// <summary>
        ///     Creates an Backup Exception
        /// </summary>
        /// <param name="message">Exception message to show to the user.</param>
        /// <param name="exception">Inner Exception of the backup exception.</param>
        public BackupException(string message, Exception exception)
            : base(message, exception) {
        }
    }
}