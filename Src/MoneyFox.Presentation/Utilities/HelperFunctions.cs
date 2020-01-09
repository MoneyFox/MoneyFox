using System;
using System.Globalization;
using System.Linq;
using System.Text;

namespace MoneyFox.Presentation.Utilities
{
    /// <summary>
    ///     Utility methods
    /// </summary>
    public static class HelperFunctions
    {
        /// <summary>
        ///     Returns the last day of the month
        /// </summary>
        /// <returns>Last day of the month</returns>
        public static DateTime GetEndOfMonth()
        {
            DateTime today = DateTime.Today;

            return new DateTime(today.Year, today.Month, DateTime.DaysInMonth(today.Year, today.Month));
        }

        /// <summary>
        ///     Returns the decimal converted to a string in a proper format for this culture.
        /// </summary>
        /// <param name="value">decimal who shall be converted</param>
        /// <returns>Formated string.</returns>
        public static string FormatLargeNumbers(decimal value)
        {
            return value.ToString("N2", CultureInfo.CurrentCulture);
        }

        /// <summary>
        ///     Returns the number string with just his culture's decimal separator.
        ///     If it is an Int it will return the same string as entered.
        /// </summary>
        /// <param name="amount">Amount to be converted.</param>
        /// <returns>Formatted string.</returns>
        public static string RemoveGroupingSeparators(string amount)
        {
            if (amount.Any(char.IsPunctuation))
            {
                var decimalSeparatorIndex = 0;
                var punctuationCount = 0;
                var stringBuilder = new StringBuilder();

                foreach (char c in amount)
                {
                    if (!char.IsPunctuation(c))
                        stringBuilder.Append(c);
                    else
                    {
                        punctuationCount++;
                        if (amount.IndexOf(c) >= amount.Length - 3)
                        {
                            decimalSeparatorIndex = amount.IndexOf(c);
                            punctuationCount--;
                        }
                    }
                }

                if (punctuationCount > decimalSeparatorIndex) return amount;

                string decimalsString = stringBuilder.ToString();
                amount = decimalsString.Substring(0, decimalSeparatorIndex - punctuationCount) +
                         CultureInfo.CurrentCulture.NumberFormat.CurrencyDecimalSeparator +
                         decimalsString.Substring(decimalSeparatorIndex - punctuationCount);
            }

            return amount;
        }
    }
}
