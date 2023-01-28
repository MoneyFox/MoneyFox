namespace MoneyFox.Ui.Converter;

using System.Globalization;
using Views.Categories;

public class NoCategorySelectedConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        var category = (CategoryListItemViewModel)value;

        return category.Name;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotSupportedException();
    }
}
