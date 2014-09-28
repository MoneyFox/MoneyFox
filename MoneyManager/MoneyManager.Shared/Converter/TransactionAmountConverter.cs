using System;
using Windows.UI.Xaml.Data;
using MoneyManager.Src;

namespace MoneyManager.Converter
{
    public class TransactionAmountConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            return (int) value == (int) TransactionType.Spending
                ? "-"
                : "+";
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
