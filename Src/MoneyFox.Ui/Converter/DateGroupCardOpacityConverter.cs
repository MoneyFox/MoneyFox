namespace MoneyFox.Ui.Converter;

using System.Globalization;

public class DateGroupCardOpacityConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        // Returns 40% opacity if the date is in the future, 100% if it's not
        return value is DateTime dt && dt > DateTime.Today ? 0.4 : 1.0;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotSupportedException();
    }
}
