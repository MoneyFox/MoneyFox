namespace MoneyFox.Win.Converter;

using Core._Pending_;
using Microsoft.UI.Xaml.Data;
using System;
using Core.Common;

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