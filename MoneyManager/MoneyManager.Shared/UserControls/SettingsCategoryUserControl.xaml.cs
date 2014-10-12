using Microsoft.Practices.ServiceLocation;
using MoneyManager.DataAccess;
using MoneyManager.Dialogs;
using MoneyManager.Models;
using System;
using Windows.UI.Xaml;
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

        private void CategoryListHolding(object sender, HoldingRoutedEventArgs e)
        {
            var senderElement = sender as FrameworkElement;
            var flyoutBase = FlyoutBase.GetAttachedFlyout(senderElement);

            flyoutBase.ShowAt(senderElement);
        }

        private async void EditCategory(object sender, RoutedEventArgs e)
        {
            var element = (FrameworkElement)sender;
            var category = element.DataContext as Category;
            if (category == null) return;

            var viewModel = ServiceLocator.Current.GetInstance<CategoryDataAccess>();
            viewModel.SelectedCategory = category;

            var dialog = new CategoryDialog(true);
            await dialog.ShowAsync();
        }

        private void DeleteCategory(object sender, RoutedEventArgs e)
        {
            var element = (FrameworkElement)sender;
            var category = element.DataContext as Category;
            if (category == null) return;

            ServiceLocator.Current.GetInstance<CategoryDataAccess>().Delete(category);
        }
    }
}