using MoneyFox.Uwp.ViewModels.Categories;
using Windows.UI.Xaml.Controls;

#nullable enable
namespace MoneyFox.Uwp.Views.Payments
{
    public sealed partial class SelectCategoryDialog
    {
        private SelectCategoryListViewModel ViewModel => (SelectCategoryListViewModel)DataContext;

        public SelectCategoryDialog()
        {
            InitializeComponent();
            DataContext = ViewModelLocator.SelectCategoryListVm;
            ViewModel.AppearingCommand.Execute(null);
        }

        private async void SearchTextBox_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            await ((AbstractCategoryListViewModel)DataContext).SearchCommand.ExecuteAsync(SearchTextBox.Text);
        }

        private void ListView_ItemClick(object sender, ItemClickEventArgs e)
        {
            ViewModel.ItemClickCommand.Execute(e.ClickedItem);
            Hide();
        }
    }
}