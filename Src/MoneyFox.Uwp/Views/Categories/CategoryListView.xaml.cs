using MoneyFox.Core.Resources;
using MoneyFox.Uwp.Services;
using MoneyFox.Uwp.ViewModels.Categories;
using System;
using Windows.UI.Xaml;

#nullable enable
namespace MoneyFox.Uwp.Views.Categories
{
    public sealed partial class CategoryListView
    {
        public override string Header => Strings.CategoriesTitle;

        private CategoryListViewModel ViewModel => (CategoryListViewModel)DataContext;

        public CategoryListView()
        {
            InitializeComponent();
            DataContext = ViewModelLocator.CategoryListVm;
        }

        private async void AddNewCategoryClick(object sender, RoutedEventArgs e)
            => await new AddCategoryDialog { RequestedTheme = ThemeSelectorService.Theme }.ShowAsync();
    }
}