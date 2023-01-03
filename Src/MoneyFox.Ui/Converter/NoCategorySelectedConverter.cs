namespace MoneyFox.Ui.Converter;

using System.Globalization;
using Resources.Strings;
using ViewModels.Categories;

public class NoCategorySelectedConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        var category = (CategoryViewModel)value;
        if (category == null)
        {
            return Translations.SelectCategoryLabel;
        }

        return category.Name;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotSupportedException();
    }
}
