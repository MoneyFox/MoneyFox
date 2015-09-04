using System;
using Windows.UI.Xaml.Data;

namespace MoneyManager.Windows.Converter
{
    public class ThemedImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var formatString = parameter as string;

            if (string.IsNullOrEmpty(formatString))
            {
                formatString = value as string;
            }

            return ThemedImageConverterLogic.GetImage(formatString);
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}