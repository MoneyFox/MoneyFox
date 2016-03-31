using System;
using Windows.UI.Xaml.Data;
using Microsoft.Practices.ServiceLocation;
using MoneyFox.Core;
using MoneyFox.Core.Interfaces;
using MoneyFox.Core.ViewModels.Models;

namespace MoneyFox.Windows.Converter
{
    public class PaymentAmountConverter : IValueConverter
    {
        private const string IGNORE_TRANSFER = "IgnoreTransfer";

        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var paymentViewModel = (PaymentViewModel) value;
            var param = parameter as string;
            string sign;

            if (paymentViewModel.Type == PaymentType.Transfer)
            {
                if (param == IGNORE_TRANSFER)
                {
                    sign = "-";
                }
                else
                {
                    sign = paymentViewModel.ChargedAccount ==
                           ServiceLocator.Current.GetInstance<IAccountRepository>().Selected
                        ? "-"
                        : "+";
                }
            }
            else
            {
                sign = paymentViewModel.Type == PaymentType.Expense
                    ? "-"
                    : "+";
            }

            return sign + " " + $"{paymentViewModel.Amount:C2}";
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}