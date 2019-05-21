using System;
using System.Globalization;
using System.Linq;

namespace MoneyFox.ServiceLayer.Utilities
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
            var today = DateTime.Today;
            return new DateTime(today.Year, today.Month, DateTime.DaysInMonth(today.Year, today.Month));
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
                int decimalSeparatorIndex = 0;
                int punctuationCount = 0;
                string decimalsString = "";

                foreach (char c in amount)
                {
                    if (!Char.IsPunctuation(c))
                    {
                        decimalsString += c;

                    } else
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

                amount = decimalsString.Substring(0, decimalSeparatorIndex - punctuationCount) +
                         CultureInfo.CurrentCulture.NumberFormat.CurrencyDecimalSeparator +
                         decimalsString.Substring(decimalSeparatorIndex - punctuationCount);
            }
            return amount;
        }
    }
}