using System;
using Windows.UI.Xaml;
using Microsoft.Practices.ServiceLocation;
using MoneyManager.Core.ViewModels;
using MoneyManager.Windows.Views.Dialogs;

namespace MoneyManager.Windows.Views
{
    public sealed partial class SettingsView
    {
        public SettingsView()
        {
            InitializeComponent();

            DataContext = ServiceLocator.Current.GetInstance<SettingDefaultsViewModel>();
        }

        private async void AddCategory(object sender, RoutedEventArgs e)
        {
            await new ModifyCategoryDialog().ShowAsync();
        }
    }
}