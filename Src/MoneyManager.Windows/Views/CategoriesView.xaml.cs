using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using MoneyManager.Core.ViewModels;
using MoneyManager.Windows.Dialogs;
using MvvmCross.Platform;

namespace MoneyManager.Windows.Views
{
    public sealed partial class CategoriesView
    {
        public CategoriesView()
        {
            InitializeComponent();
            CategoryListUserControl.DataContext = Mvx.Resolve<CategoryListViewModel>();
        }

        private async void AddCategory(object sender, RoutedEventArgs e)
        {
            await new ModifyCategoryDialog().ShowAsync();
        }

    }
}
