using System;
using Windows.UI.Xaml.Data;
using MoneyManager.Src;

namespace MoneyManager.Converter
{
    public class RecurrenceConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            return Utilities.GetTranslation(Int32.Parse(value.ToString()) == (int)TransactionRecurrence.Monthly 
                ? "MonthlyLabel" 
                : "WeeklyLabel");
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
