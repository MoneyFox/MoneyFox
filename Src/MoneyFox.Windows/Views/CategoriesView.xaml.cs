using System;
using Windows.UI.Xaml;
using MoneyFox.Shared.ViewModels;
using MoneyFox.Windows.Views.Dialogs;
using MvvmCross.Platform;

namespace MoneyFox.Windows.Views
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