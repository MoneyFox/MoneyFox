using System;
using System.Globalization;
using MvvmCross.Platform.Converters;

namespace MoneyFox.Shared.Converter {
    /// <summary>
    ///     Displays the amount as currency of the current culture.
    /// </summary>
    public class AmountFormatConverter : IMvxValueConverter {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) => $"{value:C2}";

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => value;
    }
}