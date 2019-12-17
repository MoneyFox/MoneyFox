using System;
using System.Globalization;
using MoneyFox.Application.Common.ConverterLogic;
using MoneyFox.Presentation.ViewModels;
using Xamarin.Forms;

namespace MoneyFox.Presentation.Converter
{
    /// <summary>
    ///     Converts a PaymentViewModel for displaying on the GUI.
    /// </summary>
    public class PaymentAmountConverter : IValueConverter
    {
        /// <summary>
        ///     Adds a plus or a minus to the payment amont on the UI based on if it is a income or a expense
        /// </summary>
        /// <param name="value">PaymentViewModel to convert..</param>
        /// <param name="targetType">Not used.</param>
        /// <param name="parameter">Ignore transfer parameter.</param>
        /// <param name="culture">Not used..</param>
        /// <returns>The convert.</returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
            => PaymentAmountConverterLogic.GetFormattedAmountString(value as PaymentViewModel, parameter as string);

        /// <summary>
        ///     Not Implemented.
        /// </summary>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => throw new NotSupportedException();
    }
}
