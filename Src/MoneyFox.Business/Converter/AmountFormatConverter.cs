using System;
using System.Globalization;
using MvvmCross.Platform.Converters;

namespace MoneyFox.Business.Converter
{
    /// <summary>
    ///     Displays the amount as currency of the current culture.
    /// </summary>
    public class AmountFormatConverter : IMvxValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            double currencyValue = (double)value;
            NumberFormatInfo number = culture.NumberFormat;
            number.CurrencyNegativePattern = 1;                     // formats negative values to have the "-" symbol in front of its culture
            return currencyValue.ToString("C", number);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => value;
    }
}