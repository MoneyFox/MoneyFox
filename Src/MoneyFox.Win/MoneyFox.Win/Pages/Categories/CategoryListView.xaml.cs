using Microsoft.UI.Xaml;
using MoneyFox.Core.Resources;
using MoneyFox.Win.Services;
using MoneyFox.Win.ViewModels.Categories;
using System;

namespace MoneyFox.Win.Pages.Categories
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
        {
            var messageDialog = new AddCategoryDialog { RequestedTheme = ThemeSelectorService.Theme };
            messageDialog.XamlRoot = MainWindow.RootFrame.XamlRoot;
            await messageDialog.ShowAsync();
        }
    }
}