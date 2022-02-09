using Microsoft.UI.Xaml.Controls;
using MoneyFox.Win.ViewModels.Categories;

namespace MoneyFox.Win.Pages.Payments
{
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
}