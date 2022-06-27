namespace MoneyFox.Converter
{

    using System;
    using System.Globalization;
    using Core.Common.Helpers;
    using Xamarin.Forms;

    public class DecimalConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value is decimal decimalValue ? RoundDecimalToFive(decimalValue).ToString(CultureHelper.CurrentCulture) : value;
        }

        private static int RoundDecimalToFive(decimal decimalValue)
        {
            return 5 * (int)Math.Round(d: decimalValue / 5);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (decimal.TryParse(s: value as string, style: NumberStyles.Currency, provider: CultureHelper.CurrentCulture, result: out var dec))
            {
                return dec;
            }

            return value;
        }
    }

}
