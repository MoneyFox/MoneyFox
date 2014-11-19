using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Microsoft.Practices.ServiceLocation;
using MoneyManager.DataAccess.DataAccess;
using MoneyManager.Views;

namespace MoneyManager.UserControls
{
    public sealed partial class SelectCategoryUserControl
    {
        public SelectCategoryUserControl()
        {
            InitializeComponent();
        }

        private void ResetCategory(object sender, TappedRoutedEventArgs e)
        {
            ServiceLocator.Current.GetInstance<TransactionDataAccess>().SelectedTransaction.Category = null;
        }

        private void OpenSelectCategoryDialog(object sender, RoutedEventArgs routedEventArgs)
        {
            ((Frame)Window.Current.Content).Navigate(typeof(SelectCategory));
        }
    }
}
