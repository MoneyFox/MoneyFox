using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace MoneyManager.UserControls
{
    public sealed partial class SelectCategoryUserControl
    {
        public SelectCategoryUserControl()
        {
            InitializeComponent();
        }

        private void SelectCategory(object sender, SelectionChangedEventArgs e)
        {
            ((Frame) Window.Current.Content).GoBack();
        }
    }
}