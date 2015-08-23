using System;
using Windows.UI.Xaml;
using MoneyManager.Windows.Dialogs;

namespace MoneyManager.Windows.Views
{
    public sealed partial class SelectCategoryView
    {
        public SelectCategoryView()
        {
            InitializeComponent();
        }

        private async void AddCategory(object sender, RoutedEventArgs e)
        {
            await new CategoryDialog().ShowAsync();
        }
    }
}