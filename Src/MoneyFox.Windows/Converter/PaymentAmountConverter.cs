using System;
using System.Globalization;
using MoneyFox.Foundation;
using MoneyFox.ServiceLayer.ViewModels;
using MvvmCross.Converters;
using Xamarin.Forms;

namespace MoneyFox.Windows.Converter
{
    /// <summary>
    ///     Converts a PaymentViewModel for displaying on the GUI.
    /// </summary>
    public class PaymentAmountConverter : IMvxValueConverter, IValueConverter
    {
        private const string IGNORE_TRANSFER = "IgnoreTransfer";

        /// <summary>
        ///     Adds a plus or a minus to the payment amont on the UI based on if it is a income or a expense
        /// </summary>
        /// <param name="value">PaymentViewModel to convert..</param>
        /// <param name="targetType">Not used.</param>
        /// <param name="parameter">Ignore transfer parameter.</param>
        /// <param name="culture">Not used..</param>
        /// <returns>The convert.</returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) return string.Empty;

            var payment = (PaymentViewModel) value;
            var param = parameter as string;
            string sign;

            if (payment.Type == PaymentType.Transfer)
            {
                if (param == IGNORE_TRANSFER)
                {
                    sign = "-";
                }
                else
                {
                    sign = payment.ChargedAccountId == payment.CurrentAccountId
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

        /// <summary>
        ///     Not Implemented.
        /// </summary>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}