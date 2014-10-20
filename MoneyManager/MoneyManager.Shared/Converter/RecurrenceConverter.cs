using MoneyManager.Src;
using System;
using Windows.UI.Xaml.Data;

namespace MoneyManager.Converter
{
    public class RecurrenceConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var enumInt = Int32.Parse(value.ToString());
            
            Switch(enumInt){
                case (int)TransactionRecurrence.Daily:
                    return Utilities.GetTranslation("DailyLabel");
                    
                case (int)TransactionRecurrence.Weekly:
                    return Utilities.GetTranslation("WeeklyLabel");
                    
                case (int)TransactionRecurrence.Monthly:
                    return Utilities.GetTranslation("MonthlyLabel");
                    
                case (int)TransactionRecurrence.Yearly:
                    return Utilities.GetTranslation("YearlyLabel");
                    
                Default:
                    return Utilities.GetTranslation("NoneLabel"); 
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
