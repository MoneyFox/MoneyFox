using System;
using System.Globalization;
using System.Runtime.Serialization;

namespace MoneyFox.Domain.Exceptions
{
    /// <summary>
    ///     A custom format exception who contains more specific information.
    /// </summary>
    [Serializable]
    public class ExtendedFormatException : Exception
    {
        public ExtendedFormatException()
        {
        }

        /// <summary>
        ///     Creates an Format Exception and will add additional information about your culture.
        /// </summary>
        /// <param name="message">Exception message to show to the user.</param>
        public ExtendedFormatException(string message) :
            base(GetMessageWithRegionInfo(message, string.Empty))
        {
        }

        /// <summary>
        ///     Creates an Format Exception and will add additional information about your culture.
        /// </summary>
        /// <param name="exception">Inner Exception of the backup exception.</param>
        /// <param name="textToParse">The text the system couldn't parse.</param>
        public ExtendedFormatException(Exception exception, string textToParse)
            : base(GetMessageWithRegionInfo(exception?.Message ?? "", textToParse), exception)
        {
        }

        public ExtendedFormatException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected ExtendedFormatException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        private static string GetMessageWithRegionInfo(string message, string textToParse)
            => "Text to parse: " + textToParse + Environment.NewLine +
               "Region: " + CultureInfo.CurrentCulture.DisplayName + Environment.NewLine +
               "CultureName: " + CultureInfo.CurrentCulture.Name + Environment.NewLine +
               "Numberformat: " + CultureInfo.CurrentCulture.NumberFormat + Environment.NewLine +
               message;
    }
}