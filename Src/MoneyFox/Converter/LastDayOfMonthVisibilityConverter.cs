namespace MoneyFox.Converter
{

    using System;
    using Xamarin.Forms;
    using System.Globalization;
    using Core._Pending_.Common.Helpers;
    using Core.Aggregates.Payments;

    public class LastDayOfMonthVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) =>
            value is PaymentRecurrence recurrence
               ? RecurringPaymentHelper.AllowLastDayOfMonth(recurrence)
               : false;

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) =>
            throw new NotSupportedException();
    }
}
