namespace MoneyFox.Win.Pages.Payments;

using Microsoft.UI.Xaml.Controls;
using ViewModels.Categories;

public sealed partial class SelectCategoryDialog
{
    public SelectCategoryDialog()
    {
        XamlRoot = MainWindow.RootFrame.XamlRoot;
        InitializeComponent();
        DataContext = App.GetViewModel<SelectCategoryListViewModel>();
        ViewModel.AppearingCommand.Execute(null);
    }

    private SelectCategoryListViewModel ViewModel => (SelectCategoryListViewModel)DataContext;

    private void ListView_ItemClick(object sender, ItemClickEventArgs e)
    {
        ViewModel.ItemClickCommand.Execute(e.ClickedItem);
        Hide();
    }
}
