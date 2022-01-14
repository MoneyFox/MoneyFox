using MoneyFox.Uwp.ViewModels.Accounts;
using Windows.UI.Xaml.Controls;

#nullable enable
namespace MoneyFox.Uwp.Views.Accounts
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