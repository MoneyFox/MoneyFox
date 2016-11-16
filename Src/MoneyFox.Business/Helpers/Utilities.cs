using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using MoneyFox.Foundation.Models;

namespace MoneyFox.Business.Helpers
{
    public class Utilities
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
        ///     Will round all values of the passed statistic item list
        /// </summary>
        /// <param name="items">List of statistic items.</param>
        public static void RoundStatisticItems(List<StatisticItem> items)
        {
            foreach (var item in items)
            {
                item.Value = Math.Round(item.Value, 2, MidpointRounding.AwayFromZero);
            }
        }

        /// <summary>
        ///     Returns the double converted to a string in a proper format for this culture.
        /// </summary>
        /// <param name="value">Double who shall be converted</param>
        /// <returns>Formated string.</returns>
        public static string FormatLargeNumbers(double value)
            => value.ToString("N", CultureInfo.CurrentCulture);

        /// <summary>
        ///     Returns the number string with just this culture's decimal separator.
        /// </summary>
        /// <param name="amount">Amount to be converted.</param>
        /// <returns>Formated string.</returns>
        public static string RemoveGroupingSeparators(string amount)
        {
            if (amount.Any(Char.IsPunctuation))
            {
                int decimalSeparatorIndex = 0;
                int punctuationCount = 0;
                string decimalsString = "";

                foreach (char c in amount)
                {
                    if (!Char.IsPunctuation(c))
                    {
                        decimalsString += c;

                    }
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
                amount = decimalsString.Substring(0, decimalSeparatorIndex - punctuationCount) + CultureInfo.CurrentCulture.NumberFormat.CurrencyDecimalSeparator.ToString() + decimalsString.Substring(decimalSeparatorIndex - punctuationCount);
            }
            return amount;
        }
    }
}