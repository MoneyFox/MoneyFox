using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Input;
using Cirrious.CrossCore;
using MoneyManager.Core.ViewModels;
using MoneyManager.Foundation.Model;
using MoneyManager.Windows.Dialogs;

namespace MoneyManager.Windows.Controls
{
    public partial class CategoryListUserControl
    {
        public static readonly DependencyProperty IsSettingsCallProperty =
            DependencyProperty.Register("IsSettingsCall", typeof(bool), typeof(CategoryListUserControl), null);

        public CategoryListUserControl()
        {
            InitializeComponent();
            var viewmodel = Mvx.Resolve<CategoryListViewModel>();
            viewmodel.IsSettingCall = IsSettingsCall;

            DataContext = viewmodel;
        }

        public bool IsSettingsCall
        {
            get { return (bool)GetValue(IsSettingsCallProperty); }
            set { SetValue(IsSettingsCallProperty, value); }
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

            var dialog = new CategoryDialog(category);
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

            ((CategoryListViewModel) DataContext).DeleteCategoryCommand.Execute(category);
        }
    }
}