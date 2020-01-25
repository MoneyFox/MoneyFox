using System;
using System.Runtime.Serialization;

namespace MoneyFox.Domain.Exceptions
{
    /// <summary>
    ///     This Exception is thrown when there was an issue with an internet connection.
    /// </summary>
    [Serializable]
    public class NetworkConnectionException : Exception
    {
        /// <summary>
        ///     Creates an network connection Exception
        /// </summary>
        public NetworkConnectionException()
        { }

        /// <summary>
        ///     Creates an network connection Exception
        /// </summary>
        /// <param name="message">Exception message to show to the user.</param>
        public NetworkConnectionException(string message)
            : base(message)
        { }

        /// <summary>
        ///     Creates an network connection Exception
        /// </summary>
        /// <param name="message">Exception message to show to the user.</param>
        /// <param name="exception">Inner Exception of the backup exception.</param>
        public NetworkConnectionException(string message, Exception exception) : base(message, exception)
        { }

        protected NetworkConnectionException(SerializationInfo info, StreamingContext context) : base(info, context)
        { }
    }
}
