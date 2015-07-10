using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Input;
using Microsoft.Practices.ServiceLocation;
using MoneyManager.Business.Logic;
using MoneyManager.Dialogs;
using MoneyManager.Foundation.Model;
using MoneyManager.Foundation.OperationContracts;

namespace MoneyManager.UserControls
{
    public partial class CategoryListUserControl
    {
        public CategoryListUserControl()
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
            var element = (FrameworkElement) sender;
            var category = element.DataContext as Category;
            if (category == null)
            {
                return;
            }

            var repository = ServiceLocator.Current.GetInstance<IRepository<Category>>();
            repository.Selected = category;

            var dialog = new CategoryDialog(true);
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

            CategoryLogic.DeleteCategory(category);
        }
    }
}