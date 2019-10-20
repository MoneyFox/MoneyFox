using System;
using Windows.UI.Xaml.Data;
using MoneyFox.Domain;
using MoneyFox.Presentation.Converter;

namespace MoneyFox.Uwp.Converter
{
    public class NativeRecurrenceTypeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            return RecurrenceTypeConverterLogic.GetStringForPaymentRecurrence((PaymentRecurrence) value);
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
