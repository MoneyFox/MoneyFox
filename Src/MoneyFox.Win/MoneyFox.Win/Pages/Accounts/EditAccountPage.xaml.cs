using Microsoft.UI.Xaml.Controls;
using MoneyFox.Win.ViewModels.Accounts;

namespace MoneyFox.Win.Pages.Accounts
{
    public sealed partial class EditAccountPage : ContentDialog
    {
        private EditAccountViewModel ViewModel => (EditAccountViewModel)DataContext;

        public EditAccountPage(int accountId)
        {
            InitializeComponent();
            DataContext = ViewModelLocator.EditAccountVm;
            ViewModel.AccountId = accountId;
        }
    }
}