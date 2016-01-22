using System;
using Windows.UI.Xaml;
using MoneyManager.Core.ViewModels.CategoryList;
using MoneyManager.Windows.Dialogs;
using MvvmCross.Platform;

namespace MoneyManager.Windows.Views
{
    public sealed partial class SettingsView
    {
        public SettingsView()
        {
            InitializeComponent();

            CategoryListUserControl.DataContext = Mvx.Resolve<SettingsCategoryListViewModel>();
        }

        private async void AddCategory(object sender, RoutedEventArgs e)
        {
            await new ModifyCategoryDialog().ShowAsync();
        }
    }
}