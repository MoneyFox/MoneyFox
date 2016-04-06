using System;

namespace MoneyFox.Shared.Exceptions
{
    public class ConnectionException : Exception
    {
        /// <summary>
        ///     Creates an Connection Exception
        /// </summary>
        /// <param name="message">Exception message to show to the user.</param>
        public ConnectionException(string message)
            : base(message)
        {
        }

        /// <summary>
        ///     Creates an Connection Exception
        /// </summary>
        /// <param name="message">Exception message to show to the user.</param>
        /// <param name="exception">Inner Exception of the connection exception.</param>
        public ConnectionException(string message, Exception exception)
            : base(message, exception)
        {
        }
    }
}