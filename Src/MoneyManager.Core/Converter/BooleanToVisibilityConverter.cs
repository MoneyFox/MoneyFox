using System;
using System.Globalization;
using Cirrious.CrossCore.Converters;
using Cirrious.CrossCore.UI;

namespace MoneyManager.Core.Converter
{
    public class BooleanToVisibilityConverter : IMvxValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (parameter != null && parameter.ToString() == "revert")
            {
                return (bool) value ? MvxVisibility.Collapsed : MvxVisibility.Visible;
            }

            return (bool) value ? MvxVisibility.Visible : MvxVisibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (bool) value ? MvxVisibility.Collapsed : MvxVisibility.Visible;
        }
    }
}