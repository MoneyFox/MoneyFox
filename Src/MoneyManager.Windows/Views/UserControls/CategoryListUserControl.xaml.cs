using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Input;
using MoneyManager.Core.ViewModels;
using MoneyManager.Foundation.Model;
using MoneyManager.Windows.Dialogs;
using MvvmCross.Platform;

namespace MoneyManager.Windows.Views.UserControls
{
    public partial class CategoryListUserControl
    {
        public CategoryListUserControl()
        {
            InitializeComponent();
        }

        private void CategoryListRightTapped(object sender, RightTappedRoutedEventArgs e)
        {
            var senderElement = sender as FrameworkElement;
            var flyoutBase = FlyoutBase.GetAttachedFlyout(senderElement);

            flyoutBase.ShowAt(senderElement);
        }

        private void CategoryListHolding(object sender, HoldingRoutedEventArgs e)
        {
            var senderElement = sender as FrameworkElement;
            var flyoutBase = FlyoutBase.GetAttachedFlyout(senderElement);

            flyoutBase.ShowAt(senderElement);
        }

        private async void EditCategory(object sender, RoutedEventArgs e)
        {
            var element = (FrameworkElement) sender;
            var category = element.DataContext as Category;
            if (category == null)
            {
                return;
            }

            var dialog = new ModifyCategoryDialog(category);
            await dialog.ShowAsync();
        }

        private void DeleteCategory(object sender, RoutedEventArgs e)
        {
            var element = (FrameworkElement) sender;
            var category = element.DataContext as Category;
            if (category == null)
            {
                return;
            }

            ((AbstractCategoryListViewModel) DataContext).DeleteCategoryCommand.Execute(category);
        }
    }
}