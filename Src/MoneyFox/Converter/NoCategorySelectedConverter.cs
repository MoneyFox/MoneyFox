namespace MoneyFox.Converter
{
    using Core.Resources;
    using System;
    using System.Globalization;
    using ViewModels.Categories;
    using Xamarin.Forms;

    public class NoCategorySelectedConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var category = (CategoryViewModel)value;

            if(category == null)
            {
                return Strings.SelectCategoryLabel;
            }

            return category.Name;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) =>
            throw new NotSupportedException();
    }
}