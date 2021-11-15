﻿using System.Globalization;

namespace MoneyFox.Utilities
{
    /// <summary>
    ///     Utility methods
    /// </summary>
    public static class HelperFunctions
    {
        /// <summary>
        ///     Returns the decimal converted to a string in a proper format for this culture.
        /// </summary>
        /// <param name="value">decimal who shall be converted</param>
        /// <returns>Formated string.</returns>
        public static string FormatLargeNumbers(decimal value) => value.ToString("N2", CultureInfo.CurrentCulture);
    }
}