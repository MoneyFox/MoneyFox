using Microsoft.UI.Xaml.Controls;
using MoneyFox.Win.ViewModels.Accounts;

namespace MoneyFox.Win.Pages.Accounts
{
    public sealed partial class EditAccountView : ContentDialog
    {
        private EditAccountViewModel ViewModel => (EditAccountViewModel)DataContext;

        public EditAccountView(int accountId)
        {
            InitializeComponent();
            DataContext = ViewModelLocator.EditAccountVm;
            ViewModel.AccountId = accountId;
        }
    }
}