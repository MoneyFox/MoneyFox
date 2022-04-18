namespace MoneyFox.Win.Converter;

using Core.Aggregates.Payments;
using Core._Pending_.Common.Helpers;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Data;
using System;

public class LastDayOfMonthVisibilityConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object? parameter, string language) =>
         value is PaymentRecurrence recurrence
            ? RecurringPaymentHelper.AllowLastDayOfMonth(recurrence) ? Visibility.Visible : Visibility.Collapsed
            : Visibility.Collapsed;

    public object ConvertBack(object value, Type targetType, object parameter, string language) =>
        throw new NotSupportedException();

}
