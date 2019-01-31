using System;
using System.Globalization;
using MvvmCross.Converters;

namespace MoneyFox.Presentation.Converter
{
    /// <summary>
    ///     Converts DateTime values to a nicer representation.
    /// </summary>
    public class DateTimeFormatConverter : IMvxValueConverter
    {
        /// <summary>
        ///     Converts the passed DateTime to a nice string.
        /// </summary>
        /// <param name="value">Object to convert.</param>
        /// <param name="targetType">Not used.</param>
        /// <param name="parameter">Not used.</param>
        /// <param name="culture">Not used.</param>
        /// <returns>The converted string.</returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
            => ((DateTime) value).ToString("D", CultureInfo.InvariantCulture);

        /// <summary>
        ///     Converts the passed string back.
        /// </summary>
        /// <param name="value">Value.</param>
        /// <param name="targetType">Not used.</param>
        /// <param name="parameter">Not used.</param>
        /// <param name="culture">Not used.</param>
        /// <returns>The converted string.</returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => System.Convert.ToDateTime(value).ToString("d", CultureInfo.InvariantCulture);
    }
}