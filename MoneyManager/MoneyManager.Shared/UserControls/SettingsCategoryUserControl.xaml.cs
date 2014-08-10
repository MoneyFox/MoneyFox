using MoneyManager.Models;
using MoneyManager.Src;
using MoneyManager.Views;
namespace MoneyManager.UserControls
{
    public sealed partial class SettingsCategoryUserControl
    {
        public SettingsCategoryUserControl()
        {
            InitializeComponent();
        }

        //TODO: Move to ViewModel
        //        private async void CategoryList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        //        {
        //            if (CategoryList.SelectedItem != null)
        //            {
        //                var category = CategoryList.SelectedItem as Category;
        //#if WINDOWS_PHONE_APP
        //                await new CategoryDialog(category).ShowAsync();
        //#endif
        //                CategoryList.SelectedItem = null;
        //            }
        //        }
    }
}