using MoneyFox.Application;
using MoneyFox.ViewModels.Accounts;
using System;
using System.Globalization;
using Xamarin.Forms;

namespace MoneyFox.Converter
{
    public class AccountNameConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return !(value is AccountViewModel account)
                   ? string.Empty : $"{account.Name} ({account.CurrentBalance.ToString("C", CultureHelper.CurrentCulture)})";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => throw new NotSupportedException();
    }
}
