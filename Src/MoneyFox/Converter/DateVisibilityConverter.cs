﻿using System;
using System.Globalization;
using Xamarin.Forms;

namespace MoneyFox.Converter
{
    /// <summary>
    ///     Hides the date if it is equal to the default DateTime Value.
    /// </summary>
    public class DateVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
            => (DateTime)value != new DateTime();

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => (DateTime)value != new DateTime();
    }
}