using System;
using Windows.UI.Xaml;
using Cirrious.CrossCore;
using MoneyManager.Core.ViewModels;
using MoneyManager.Windows.Dialogs;

namespace MoneyManager.Windows.Views
{
    public sealed partial class SelectCategoryView
    {
        public SelectCategoryView()
        {
            InitializeComponent();
            DataContext = Mvx.Resolve<SelectCategoryViewModel>();
        }

        private async void AddCategory(object sender, RoutedEventArgs e)
        {
            await new CategoryDialog().ShowAsync();
        }
    }
}