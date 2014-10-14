using Microsoft.Practices.ServiceLocation;
using MoneyManager.ViewModels;
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
            ((Frame)Window.Current.Content).GoBack();
        }

        private void SearchCategories(object sender, TextChangedEventArgs textChangedEventArgs)
        {
            //ServiceLocator.Current.GetInstance<SelectCategoryViewModel>().sea(TextBoxSearchfield.Text);
        }
    }
}