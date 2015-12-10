using System;
using Windows.UI.Xaml;
using Cirrious.CrossCore;
using MoneyManager.Core.ViewModels.CategoryList;
using MoneyManager.Core.ViewModels.SettingViews;
using MoneyManager.Windows.Dialogs;

namespace MoneyManager.Windows.Views
{
    public sealed partial class SettingsView
    {
        public SettingsView()
        {
            InitializeComponent();

            DataContext = Mvx.Resolve<SettingDefaultsViewModel>();
            CategoryListUserControl.DataContext = Mvx.Resolve<SettingsCategoryListViewModel>();
        }

        private async void AddCategory(object sender, RoutedEventArgs e)
        {
            await new ModifyCategoryDialog().ShowAsync();
        }
    }
}