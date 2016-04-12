using MvvmCross.Platform.Converters;
using System;
using System.Globalization;

namespace MoneyFox.Shared.Converter
{
    public class AmountFormatConverter : IMvxValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) => $"{value:C2}";

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => value;
    }
}