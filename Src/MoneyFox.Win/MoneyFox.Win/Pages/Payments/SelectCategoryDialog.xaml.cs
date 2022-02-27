namespace MoneyFox.Win.Pages.Payments;

using Microsoft.UI.Xaml.Controls;
using ViewModels.Categories;

public sealed partial class SelectCategoryDialog
{
    private SelectCategoryListViewModel ViewModel => (SelectCategoryListViewModel)DataContext;

    public SelectCategoryDialog()
    {
        XamlRoot = MainWindow.RootFrame.XamlRoot;
        InitializeComponent();
        DataContext = ViewModelLocator.SelectCategoryListVm;
        ViewModel.AppearingCommand.Execute(null);
    }

    private void ListView_ItemClick(object sender, ItemClickEventArgs e)
    {
        ViewModel.ItemClickCommand.Execute(e.ClickedItem);
        Hide();
    }
}