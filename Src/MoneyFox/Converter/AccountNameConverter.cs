namespace MoneyFox.Converter
{
    using Core._Pending_;
    using System;
    using System.Globalization;
    using ViewModels.Accounts;
    using Xamarin.Forms;

    public class AccountNameConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) =>
            !(value is AccountViewModel account)
                ? string.Empty
                : $"{account.Name} ({account.CurrentBalance.ToString("C", CultureHelper.CurrentCulture)})";

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => throw new NotSupportedException();
    }
}