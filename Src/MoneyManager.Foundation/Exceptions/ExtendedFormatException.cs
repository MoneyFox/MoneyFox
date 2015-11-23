using System;
using System.Globalization;

namespace MoneyManager.Foundation.Exceptions
{
    /// <summary>
    ///     A custom format exception who contains more specific informations.
    /// </summary>
    public class ExtendedFormatException : Exception
    {
        /// <summary>
        ///     Creates an Format Exception and will add additional information about your culture.
        /// </summary>
        /// <param name="message">Exception message to show to the user.</param>
        public ExtendedFormatException(string message) :
            base(GetMessageWithRegionInfo(message)) { }

        /// <summary>
        ///     Creates an Format Exception and will add additional information about your culture.
        /// </summary>
        /// <param name="message">Exception message to show to the user.</param>
        /// <param name="exception">Inner Exception of the backup exception.</param>
        public ExtendedFormatException(Exception exception)
            : base(GetMessageWithRegionInfo(exception.Message), exception)
        { }

        private static string GetMessageWithRegionInfo(string message)
        {
            return "Region: " + CultureInfo.CurrentCulture.DisplayName + Environment.NewLine +
                   "CultureName: " + CultureInfo.CurrentCulture.Name + Environment.NewLine +
                   "Numberformat: " + CultureInfo.CurrentCulture.NumberFormat + Environment.NewLine +
                   message;
        }
    }
}
