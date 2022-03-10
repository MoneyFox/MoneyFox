namespace MoneyFox.Converter
{
    using Core.Aggregates.Payments;
    using Core.Resources;
    using System;
    using System.Globalization;
    using Xamarin.Forms;

    public class PaymentTypeFilterConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var filteredPaymentType = (PaymentTypeFilter)Enum.ToObject(typeof(PaymentTypeFilter), value);

            return filteredPaymentType switch
            {
                PaymentTypeFilter.All => Strings.AllLabel,
                PaymentTypeFilter.Expense => Strings.ExpenseLabel,
                PaymentTypeFilter.Income => Strings.IncomeLabel,
                PaymentTypeFilter.Transfer => Strings.TransferLabel,
                _ => string.Empty
            };
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) =>
            throw new NotSupportedException();
    }

}
