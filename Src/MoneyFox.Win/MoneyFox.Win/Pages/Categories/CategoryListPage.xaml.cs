namespace MoneyFox.Win.Pages.Categories;

using System;
using Core.Resources;
using Microsoft.UI.Xaml;
using ViewModels.Categories;

public sealed partial class CategoryListPage
{
    public CategoryListPage()
    {
        InitializeComponent();
        DataContext = ViewModelLocator.CategoryListVm;
    }

    public override string Header => Strings.CategoriesTitle;

    private CategoryListViewModel ViewModel => (CategoryListViewModel)DataContext;

    private async void AddNewCategoryClick(object sender, RoutedEventArgs e)
    {
        var messageDialog = new AddCategoryDialog();
        await messageDialog.ShowAsync();
    }
}
