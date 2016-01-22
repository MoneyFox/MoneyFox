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
            var transaction = (Payment) value;
            string sign;

            if (transaction.Type == (int) PaymentType.Transfer)
            {
                sign = transaction.ChargedAccountId == Mvx.Resolve<IRepository<Account>>().Selected.Id
                    ? "-"
                    : "+";
            }
            else
            {
                sign = transaction.Type == (int) PaymentType.Spending
                    ? "-"
                    : "+";
            }

            return sign + " " + $"{transaction.Amount:C2}";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}