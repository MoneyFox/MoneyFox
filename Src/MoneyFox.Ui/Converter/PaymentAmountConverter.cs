namespace MoneyFox.Ui.Converter;

using System.Globalization;
using Common.ConverterLogic;
using Views.Payments;

public class PaymentAmountConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return value is PaymentViewModel model
            ? PaymentAmountConverterLogic.GetAmountSign(model)
            : string.Empty;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotSupportedException();
    }
}
