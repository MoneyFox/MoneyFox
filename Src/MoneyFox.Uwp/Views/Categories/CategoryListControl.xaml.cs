﻿using MoneyFox.Application.Common;
using MoneyFox.Uwp.ViewModels.Categories;
using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Input;

#nullable enable
namespace MoneyFox.Uwp.Views.Categories
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

            ((AbstractCategoryListViewModel)DataContext).DeleteCategoryCommand.ExecuteAsync(category)
                                                        .FireAndForgetSafeAsync();
        }

        private void SearchTextBox_OnTextChanged(object sender, TextChangedEventArgs e)
            => ((AbstractCategoryListViewModel)DataContext).SearchCommand.ExecuteAsync(SearchTextBox.Text)
                                                           .FireAndForgetSafeAsync();
    }
}