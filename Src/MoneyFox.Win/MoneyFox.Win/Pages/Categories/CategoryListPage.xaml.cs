namespace MoneyFox.Win.Pages.Categories;

using Core.Resources;
using Microsoft.UI.Xaml;
using System;
using ViewModels.Categories;

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