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
            NumberFormatInfo noParens = (NumberFormatInfo)culture.NumberFormat.Clone();
            noParens.CurrencyNegativePattern = 1;
            double currencyValue = (double)value;
            return currencyValue.ToString("C", noParens);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => value;
    }
}