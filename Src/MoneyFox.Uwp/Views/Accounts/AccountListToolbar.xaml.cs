using MoneyFox.Uwp.ViewModels.Interfaces;
using System;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

namespace MoneyFox.Uwp.Views.Accounts
{
    public sealed partial class AccountListToolbar : UserControl
    {
        public IAccountListViewActionViewModel ViewModel => (IAccountListViewActionViewModel)DataContext;

        public AccountListToolbar()
        {
            InitializeComponent();
        }

        private async void AddAccountTapped(object sender, TappedRoutedEventArgs e)
        {
            await new AddAccountDialog().ShowAsync();
        }
    }
}
