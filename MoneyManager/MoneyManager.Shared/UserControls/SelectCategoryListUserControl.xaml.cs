using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Input;
using Microsoft.Practices.ServiceLocation;
using MoneyManager.Business.Logic;
using MoneyManager.DataAccess.DataAccess;
using MoneyManager.Dialogs;
using MoneyManager.Foundation.Model;

namespace MoneyManager.UserControls {
    public partial class CategoryListUserControl {
        public CategoryListUserControl() {
            InitializeComponent();
        }

        private void CategoryListHolding(object sender, HoldingRoutedEventArgs e) {
            var senderElement = sender as FrameworkElement;
            FlyoutBase flyoutBase = FlyoutBase.GetAttachedFlyout(senderElement);

            flyoutBase.ShowAt(senderElement);
        }

        private async void EditCategory(object sender, RoutedEventArgs e) {
            var element = (FrameworkElement) sender;
            var category = element.DataContext as Category;
            if (category == null) return;

            var viewModel = ServiceLocator.Current.GetInstance<CategoryDataAccess>();
            viewModel.SelectedCategory = category;

            var dialog = new CategoryDialog(true);
            await dialog.ShowAsync();
        }

        private void DeleteCategory(object sender, RoutedEventArgs e) {
            var element = (FrameworkElement) sender;
            var category = element.DataContext as Category;
            if (category == null) return;

            CategoryLogic.DeleteCategory(category);
        }
    }
}