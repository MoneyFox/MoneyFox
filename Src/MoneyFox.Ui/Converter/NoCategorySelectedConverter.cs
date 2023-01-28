namespace MoneyFox.Ui.Converter;

using System.Globalization;
using Resources.Strings;
using Views.Categories;
using Views.Categories.ModifyCategory;

public class NoCategorySelectedConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        var category = value as CategoryViewModel;
        return category == null ? Translations.SelectCategoryLabel : category.Name;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotSupportedException();
    }
}
