using MoneyFox.Application.Resources;
using MoneyFox.Ui.Shared.ViewModels.Categories;
using System;
using System.Globalization;
using Xamarin.Forms;

namespace MoneyFox.Converter
{
    public class NoCategorySelectedConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var category = (CategoryViewModel)value;

            if(category == null)
                return Strings.SelectCategoryTitle;
            return category.Name;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
