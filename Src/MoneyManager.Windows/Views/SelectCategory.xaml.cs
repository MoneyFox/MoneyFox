using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Navigation;
using MoneyManager.Windows.Dialogs;

namespace MoneyManager.Windows.Views
{
    public sealed partial class SelectCategory
    {

        public SelectCategory()
        {
            InitializeComponent();
        }

        private async void AddCategory(object sender, RoutedEventArgs e)
        {
            await new CategoryDialog().ShowAsync();
        }
    }
}