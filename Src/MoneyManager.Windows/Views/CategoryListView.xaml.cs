using System;
using Windows.UI.Xaml;
using Cirrious.CrossCore;
using MoneyManager.Core.ViewModels;
using MoneyManager.Windows.Dialogs;

namespace MoneyManager.Windows.Views
{
    public sealed partial class CategoryListView
    {
        public CategoryListView()
        {
            InitializeComponent();
            Mvx.Resolve<CategoryListViewModel>().IsSettingCall = false;
            DataContext = Mvx.Resolve<CategoryListViewModel>();
        }

        private async void AddCategory(object sender, RoutedEventArgs e)
        {
            await new CategoryDialog().ShowAsync();
        }
    }
}