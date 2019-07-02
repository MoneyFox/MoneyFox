using System;
using System.Runtime.Serialization;

namespace MoneyFox.Domain.Exceptions
{
    [Serializable]
    public class AccountMissingException : Exception
    {
        /// <summary>
        ///     Creates an AccountMissingException Exception
        /// </summary>
        public AccountMissingException()
        {
        }

        /// <summary>
        ///     Creates an AccountMissingException Exception
        /// </summary>
        /// <param name="message">Exception message to show to the user.</param>
        public AccountMissingException(string message)
            : base(message)
        {
        }

        /// <summary>
        ///     Creates an AccountMissingException Exception
        /// </summary>
        /// <param name="message">Exception message to show to the user.</param>
        /// <param name="exception">Inner Exception of the backup exception.</param>
        public AccountMissingException(string message, Exception exception) : base(message, exception)
        {
        }

        protected AccountMissingException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}