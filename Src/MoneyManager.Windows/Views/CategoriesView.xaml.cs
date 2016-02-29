using System;
using Windows.UI.Xaml;
using MoneyManager.Windows.Views.Dialogs;

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