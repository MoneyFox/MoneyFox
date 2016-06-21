using System;
using System.Globalization;
using MoneyFox.Shared.Interfaces;
using MoneyFox.Shared.Model;
using MvvmCross.Platform;
using MvvmCross.Platform.Converters;

namespace MoneyFox.Shared.Converter {
    public class PaymentAmountConverter : IMvxValueConverter {
        private const string IGNORE_TRANSFER = "IgnoreTransfer";

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
            var payment = (Payment) value;
            var param = parameter as string;
            string sign;

            if (payment.Type == (int) PaymentType.Transfer) {
                if (param == IGNORE_TRANSFER) {
                    sign = "-";
                }
                else {
                    sign = payment.ChargedAccountId == Mvx.Resolve<IAccountRepository>().Selected.Id
                        ? "-"
                        : "+";
                }
            }
            else {
                sign = payment.Type == (int) PaymentType.Expense
                    ? "-"
                    : "+";
            }

            return sign + " " + $"{payment.Amount:C2}";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
            throw new NotImplementedException();
        }
    }
}