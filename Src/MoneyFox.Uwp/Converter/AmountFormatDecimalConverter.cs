using MoneyFox.Core._Pending_;
using System;
using Windows.UI.Xaml.Data;

#nullable enable
namespace MoneyFox.Uwp.Converter
{
    public class AmountFormatDecimalConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            decimal currencyValue = (decimal)value;
            return currencyValue.ToString("C", CultureHelper.CurrentCulture);
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language) =>
            throw new NotSupportedException();
    }
}