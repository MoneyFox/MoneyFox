﻿using MoneyFox.Application;
using System;
using System.Globalization;
using Xamarin.Forms;

namespace MoneyFox.Converter
{
    /// <summary>
    ///     Displays the amount as currency of the current culture.
    /// </summary>
    public class AmountFormatConverter : IValueConverter
    {
        /// <summary>
        ///     Converts the passed value to a currency string.
        /// </summary>
        /// <param name="value">value to convert</param>
        /// <param name="targetType">Is not used.</param>
        /// <param name="parameter">Is not used.</param>
        /// <param name="culture">Culture to use to convert.</param>
        /// <returns>Converted currency string.</returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            decimal currencyValue = (decimal)value;
            return currencyValue.ToString("C", CultureHelper.CurrentCulture);
        }

        /// <summary>
        ///     Returns the value.
        /// </summary>
        /// <returns>Passed value.</returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => value;
    }
}