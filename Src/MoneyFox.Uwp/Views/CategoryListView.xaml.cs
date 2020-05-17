using MoneyFox.Application.Resources;
using System;

namespace MoneyFox.Uwp.Views
{
    public sealed partial class CategoryListView
    {
        public override string Header => Strings.CategoriesTitle;

        public CategoryListView()
        {
            InitializeComponent();
        }

        private async void AddNewCategoryClick(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            await new AddCategoryDialog().ShowAsync();
        }
    }
}
