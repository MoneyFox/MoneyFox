﻿using MoneyFox.ConverterLogic;
using MoneyFox.Domain;
using System;
using System.Globalization;
using Xamarin.Forms;

namespace MoneyFox.Converter
{
    /// <summary>
    ///     Converts the RecurrenceType to a string.
    /// </summary>
    public class RecurrenceTypeConverter : IValueConverter
    {
        /// <summary>
        ///     Converts the passed recurrence type to a string.
        /// </summary>
        /// <param name="value">Recurrence type to convert.</param>
        /// <param name="targetType">Is not used.</param>
        /// <param name="parameter">Is not used.</param>
        /// <param name="culture">Is not used.</param>
        /// <returns>String for the RecurrenceType.</returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) =>
            RecurrenceTypeConverterLogic.GetStringForPaymentRecurrence((PaymentRecurrence)value);

        /// <summary>
        ///     Not implemented.
        /// </summary>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) =>
            throw new NotSupportedException();
    }
}