using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace MoneyFox.Windows.Converter
{
    public class BooleanToThemeConverter : IValueConverter
    {
        //if it's true, use dark theme, else use light theme
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is bool && (bool) value)
            {
                if (parameter != null)
                {
                    if (parameter.ToString() == "!")
                    {
                        return ElementTheme.Light;
                    }
                    return ElementTheme.Dark;
                }
                return ElementTheme.Dark;
            }
            if (parameter != null)
            {
                if (parameter.ToString() == "!")
                {
                    return ElementTheme.Dark;
                }
                return ElementTheme.Light;
            }
            return ElementTheme.Light;
        }

        /// <summary>
        ///     Convert visibility to boolean
        /// </summary>
        /// <param name="value">Value to convert</param>
        /// <param name="targetType">Target type</param>
        /// <param name="parameter">Optional parameter</param>
        /// <param name="language">Language used</param>
        /// <returns></returns>
        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            if (value is ElementTheme && (ElementTheme) value == ElementTheme.Dark)
            {
                return true;
            }
            return false;
        }
    }
}