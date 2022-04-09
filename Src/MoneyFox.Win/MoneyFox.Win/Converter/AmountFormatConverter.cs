namespace MoneyFox.Win.Converter;

using System;
using Core.Common;
using Microsoft.UI.Xaml.Data;

public class AmountFormatConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, string language)
    {
        var currencyValue = (decimal)value;

        return currencyValue.ToString(format: "C", provider: CultureHelper.CurrentCulture);
    }

    public object ConvertBack(object value, Type targetType, object parameter, string language)
    {
        throw new NotSupportedException();
    }
}
