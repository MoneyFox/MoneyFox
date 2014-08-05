using MoneyManager.Models;
using MoneyManager.Src;
using MoneyManager.Views;
using System;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Input;

namespace MoneyManager.UserControls
{
    public sealed partial class SettingsCategoryUserControl
    {
        public SettingsCategoryUserControl()
        {
            InitializeComponent();
        }

        private async void CategoryList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (CategoryList.SelectedItem != null)
            {
                var category = CategoryList.SelectedItem as Category;
#if WINDOWS_PHONE_APP
                await new CategoryDialog(category).ShowAsync();
#endif
                CategoryList.SelectedItem = null;
            }
        }
    }
}