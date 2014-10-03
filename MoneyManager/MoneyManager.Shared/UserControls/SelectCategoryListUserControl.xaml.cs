using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Microsoft.Practices.ServiceLocation;
using MoneyManager.ViewModels;

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
            ((Frame)Window.Current.Content).GoBack();
        }

        private void SearchCategories(object sender, TextChangedEventArgs textChangedEventArgs)
        {
            ServiceLocator.Current.GetInstance<SelectCategoryViewModel>().Search(TextBoxSearchfield.Text);
        }
    }
}
