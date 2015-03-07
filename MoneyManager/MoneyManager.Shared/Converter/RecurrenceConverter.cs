#region

using System;
using Windows.UI.Xaml.Data;
using MoneyManager.Foundation;

#endregion

namespace MoneyManager.Converter {
    public class RecurrenceConverter : IValueConverter {
        public object Convert(object value, Type targetType, object parameter, string language) {
            int enumInt = Int32.Parse(value.ToString());

            switch (enumInt) {
                case (int) TransactionRecurrence.Weekly:
                    return Translation.GetTranslation("WeeklyLabel");

                case (int) TransactionRecurrence.Monthly:
                    return Translation.GetTranslation("MonthlyLabel");

                case (int) TransactionRecurrence.Yearly:
                    return Translation.GetTranslation("YearlyLabel");
            }

            return Translation.GetTranslation("NoneLabel");
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language) {
            throw new NotImplementedException();
        }
    }
}