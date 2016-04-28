using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace MoneyFox.Windows.Converter
{
    public class BackupDateVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            DateTime backupDate = (DateTime)value;
            if(backupDate == new DateTime())
            {
                return Visibility.Collapsed;
            }
            else
            {
                return Visibility.Visible;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
