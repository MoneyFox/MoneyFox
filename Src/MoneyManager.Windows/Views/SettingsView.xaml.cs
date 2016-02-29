using System;
using Windows.UI.Xaml;
using MoneyManager.Windows.Views.Dialogs;

namespace MoneyManager.Windows.Views
{
    public sealed partial class SettingsView
    {
        public SettingsView()
        {
            InitializeComponent();

            DataContext = Mvx.Resolve<SettingDefaultsViewModel>();
        }

        private async void AddCategory(object sender, RoutedEventArgs e)
        {
            await new ModifyCategoryDialog().ShowAsync();
        }
    }
}