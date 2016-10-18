using System;
using System.Globalization;
using MoneyFox.Shared.Model;
using MvvmCross.Platform.UI;
using MvvmCross.Plugins.Color;

namespace MoneyFox.Shared.Converter
{
    public class AccountWarningColorConverter : MvxColorValueConverter
    {
        protected override MvxColor Convert(object value, object parameter, CultureInfo culture)
        {
            var input = (Account)value;
            return input.IsOverdrawn ? new MvxColor(255, 255, 255) : new MvxColor(255, 255, 69);
        }
    }
}
