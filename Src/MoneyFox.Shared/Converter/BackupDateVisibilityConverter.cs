using System;
using System.Globalization;
using MvvmCross.Platform.Converters;
using MvvmCross.Platform.UI;

namespace MoneyFox.Shared.Converter
{
    public class BackupDateVisibilityConverter : IMvxValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var backupDate = (DateTime)value;
            var test = backupDate == new DateTime() ? MvxVisibility.Collapsed : MvxVisibility.Visible;
            return test;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
