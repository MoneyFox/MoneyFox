using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Input;
using MoneyFox.Win.ViewModels.Categories;
using System;

namespace MoneyFox.Win.Pages.Categories
{
    public partial class CategoryListControl
    {
        private AbstractCategoryListViewModel ViewModel => (AbstractCategoryListViewModel)DataContext;

        public CategoryListControl()
        {
            InitializeComponent();
        }

        private void CategoryListRightTapped(object sender, RightTappedRoutedEventArgs e)
        {
            var senderElement = sender as FrameworkElement;
            var flyoutBase = FlyoutBase.GetAttachedFlyout(senderElement) as MenuFlyout;

            flyoutBase?.ShowAt(senderElement, e.GetPosition(senderElement));
        }

        private async void EditCategory(object sender, RoutedEventArgs e)
        {
            var element = (FrameworkElement)sender;

            if(!(element.DataContext is CategoryViewModel category))
            {
                return;
            }

            await new EditCategoryDialog(category.Id).ShowAsync();

            ((AbstractCategoryListViewModel)DataContext).EditCategoryCommand.Execute(category);
        }

        private void DeleteCategory(object sender, RoutedEventArgs e)
        {
            var element = (FrameworkElement)sender;

            if(!(element.DataContext is CategoryViewModel category))
            {
                return;
            }

            ViewModel.DeleteCategoryCommand.ExecuteAsync(category);
        }
    }
}