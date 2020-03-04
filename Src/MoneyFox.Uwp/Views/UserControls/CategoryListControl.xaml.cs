using MoneyFox.Ui.Shared.Utilities;
using MoneyFox.Uwp.ViewModels;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Input;

namespace MoneyFox.Uwp.Views.UserControls
{
    public partial class CategoryListControl
    {
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

        private void EditCategory(object sender, RoutedEventArgs e)
        {
            var element = (FrameworkElement) sender;
            var category = element.DataContext as CategoryViewModel;

            if(category == null)
                return;

            ((AbstractCategoryListViewModel) DataContext).EditCategoryCommand.Execute(category);
        }

        private void DeleteCategory(object sender, RoutedEventArgs e)
        {
            var element = (FrameworkElement) sender;
            var category = element.DataContext as CategoryViewModel;

            if(category == null)
                return;

            ((AbstractCategoryListViewModel) DataContext).DeleteCategoryCommand.ExecuteAsync(category).FireAndForgetSafeAsync();
        }

        private void SearchTextBox_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            ((AbstractCategoryListViewModel) DataContext).SearchCommand.ExecuteAsync(SearchTextBox.Text).FireAndForgetSafeAsync();
        }
    }
}
