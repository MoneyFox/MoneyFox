using System;
using System.Globalization;
using MoneyFox.Business.ViewModels;
using Xamarin.Forms;

namespace MoneyFox.Converter
{
    public class AccountNameConverter : IValueConverter

    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var account = value as AccountViewModel;
            return account == null ? "" : $"{account.Name} ({account.CurrentBalance:C})";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
