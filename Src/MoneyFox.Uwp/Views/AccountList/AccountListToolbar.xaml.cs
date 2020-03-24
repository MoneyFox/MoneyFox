using MoneyFox.Uwp.ViewModels.Interfaces;
using Windows.UI.Xaml.Controls;

namespace MoneyFox.Uwp.Views.AccountList
{
    public sealed partial class AccountListToolbar : UserControl
    {
        public IAccountListViewActionViewModel ViewModel => (IAccountListViewActionViewModel)DataContext;

        public AccountListToolbar()
        {
            InitializeComponent();
        }

        private void AddAccountTapped(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            ViewModel.GoToAddAccountCommand.Execute(null);
        }
    }
}
