using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using MoneyManager.Windows.Views;

namespace MoneyManager.Windows.Controls
{
    public sealed partial class SelectCategoryUserControl
    {
        public SelectCategoryUserControl()
        {
            InitializeComponent();
        }

        //TODO: Handle in View Model
        private void OpenSelectCategoryDialog(object sender, RoutedEventArgs routedEventArgs)
        {
            ((Frame) Window.Current.Content).Navigate(typeof (SelectCategoryView));
        }
    }
}