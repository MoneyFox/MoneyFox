using System;
using System.Globalization;
using MvvmCross.Platform.Converters;
using MvvmCross.Platform.UI;
using MvvmCross.Plugins.Visibility;

namespace MoneyFox.Shared.Converter
{
    public class BackupDateVisibilityConverter : MvxBaseVisibilityValueConverter
    {
        protected override MvxVisibility Convert(object value, object parameter, CultureInfo culture)
        {
            var backupDate = (DateTime)value;
            var test = backupDate == new DateTime() ? MvxVisibility.Collapsed : MvxVisibility.Visible;
            return test;
        }
    }
}
