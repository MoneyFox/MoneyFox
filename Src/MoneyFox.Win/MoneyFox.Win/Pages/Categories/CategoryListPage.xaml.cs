using Microsoft.UI.Xaml;
using MoneyFox.Core.Resources;
using MoneyFox.Win.ViewModels.Categories;
using System;

namespace MoneyFox.Win.Pages.Categories
{
    public sealed partial class CategoryListPage
    {
        public override string Header => Strings.CategoriesTitle;

        private CategoryListViewModel ViewModel => (CategoryListViewModel)DataContext;

        public CategoryListPage()
        {
            InitializeComponent();
            DataContext = ViewModelLocator.CategoryListVm;
        }

        private async void AddNewCategoryClick(object sender, RoutedEventArgs e)
        {
            var messageDialog = new AddCategoryDialog();
            await messageDialog.ShowAsync();
        }
    }
}