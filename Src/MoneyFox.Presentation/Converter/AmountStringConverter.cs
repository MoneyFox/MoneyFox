using System;
using System.Globalization;
using MoneyFox.ServiceLayer.Utilities;
using Xamarin.Forms;

namespace MoneyFox.Presentation.Converter
{
    public class AmountStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // we remove all separator chars to ensure that it works in all regions
            string amountString = HelperFunctions.RemoveGroupingSeparators((string)value);

            if (double.TryParse(amountString, NumberStyles.Any, CultureInfo.CurrentCulture, out double convertedValue))
            {
                return convertedValue;
            }

            return -1;
        }
    }
}
