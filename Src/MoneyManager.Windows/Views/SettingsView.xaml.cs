using System;
using Windows.UI.Xaml;
using MoneyManager.Core.ViewModels;
using MvvmCross.Platform;
using ModifyCategoryDialog = MoneyManager.Windows.Views.Dialogs.ModifyCategoryDialog;

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