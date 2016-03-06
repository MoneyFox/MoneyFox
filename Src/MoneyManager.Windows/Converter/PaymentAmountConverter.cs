using System;
using Windows.UI.Xaml.Data;
using Microsoft.Practices.ServiceLocation;
using MoneyFox.Foundation.Model;
using MoneyManager.Foundation;
using MoneyManager.Foundation.Interfaces;

namespace MoneyManager.Windows.Converter
{
    public class PaymentAmountConverter : IValueConverter
    {
        private const string IGNORE_TRANSFER = "IgnoreTransfer";

        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var payment = (Payment) value;
            var param = parameter as string;
            string sign;

            if (payment.Type == (int) PaymentType.Transfer)
            {
                if (param == IGNORE_TRANSFER)
                {
                    sign = "-";
                }
                else
                {
                    sign = payment.ChargedAccountId ==
                           ServiceLocator.Current.GetInstance<IAccountRepository>().Selected.Id
                        ? "-"
                        : "+";
                }
            }
            else
            {
                sign = payment.Type == (int) PaymentType.Expense
                    ? "-"
                    : "+";
            }

            return sign + " " + $"{payment.Amount:C2}";
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}