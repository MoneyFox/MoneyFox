using System;
using Windows.UI.Xaml;
using Microsoft.Practices.ServiceLocation;
using MoneyFox.Core.ViewModels;
using MoneyFox.Windows.Views.Dialogs;

namespace MoneyFox.Windows.Views
{
    public sealed partial class CategoriesView
    {
        public CategoriesView()
        {
            InitializeComponent();
            CategoryListUserControl.DataContext = ServiceLocator.Current.GetInstance<CategoryListViewModel>();
        }

        private async void AddCategory(object sender, RoutedEventArgs e)
        {
            await new ModifyCategoryDialog().ShowAsync();
        }
    }
}