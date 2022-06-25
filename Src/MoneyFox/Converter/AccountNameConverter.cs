namespace MoneyFox.Converter
{

    using System;
    using System.Globalization;
    using Core.Common;
    using Core.Common.Helpers;
    using ViewModels.Accounts;
    using Xamarin.Forms;

    public class AccountNameConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return !(value is AccountViewModel account)
                ? string.Empty
                : $"{account.Name} ({account.CurrentBalance.ToString(format: "C", provider: CultureHelper.CurrentCulture)})";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }

}
