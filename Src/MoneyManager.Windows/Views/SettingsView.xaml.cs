using System;
using Windows.UI.Xaml;
using MoneyManager.Core.ViewModels;
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
            DataContext = Mvx.Resolve<SettingDefaultsViewModel>();
        }

        private async void AddCategory(object sender, RoutedEventArgs e)
        {
            await new ModifyCategoryDialog().ShowAsync();
        }
    }
}