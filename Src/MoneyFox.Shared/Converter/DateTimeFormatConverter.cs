using System;
using System.Globalization;
using MvvmCross.Platform.Converters;

namespace MoneyFox.Shared.Converter {
    /// <summary>
    ///     Converts DateTime values to a nicer representation.
    /// </summary>
    public class DateTimeFormatConverter : IMvxValueConverter {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
            => ((DateTime) value).ToString("D");

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => System.Convert.ToDateTime(value).ToString("d");
    }
}