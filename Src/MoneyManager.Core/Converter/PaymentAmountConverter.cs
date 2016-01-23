using System;
using System.Globalization;
using MoneyManager.Foundation;
using MoneyManager.Foundation.Interfaces;
using MoneyManager.Foundation.Model;
using MvvmCross.Platform;
using MvvmCross.Platform.Converters;

namespace MoneyManager.Core.Converter
{
    public class PaymentAmountConverter : IMvxValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var payment = (Payment) value;
            string sign;

            if (payment.Type == (int) PaymentType.Transfer)
            {
                sign = payment.ChargedAccountId == Mvx.Resolve<IRepository<Account>>().Selected.Id
                    ? "-"
                    : "+";
            }
            else
            {
                sign = payment.Type == (int) PaymentType.Spending
                    ? "-"
                    : "+";
            }

            return sign + " " + $"{payment.Amount:C2}";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}