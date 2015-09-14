using System;
using Windows.UI.Xaml;
using Cirrious.CrossCore;
using MoneyManager.Core.ViewModels;
using MoneyManager.Windows.Dialogs;

namespace MoneyManager.Windows.Views
{
    public sealed partial class SettingsView
    {
        public SettingsView()
        {
            InitializeComponent();
            DataContext = Mvx.Resolve<SettingDefaultsViewModel>();

            Mvx.Resolve<CategoryListViewModel>().IsSettingCall = true;
        }

        private async void AddCategory(object sender, RoutedEventArgs e)
        {
            await new CategoryDialog().ShowAsync();
        }
    }
}