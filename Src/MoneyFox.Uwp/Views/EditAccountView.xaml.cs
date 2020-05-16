using MoneyFox.Uwp.ViewModels;
using Windows.UI.Xaml.Controls;

namespace MoneyFox.Uwp.Views
{
    public sealed partial class EditAccountView : ContentDialog
    {
        private EditAccountViewModel ViewModel => DataContext as EditAccountViewModel;

        public EditAccountView(int accountId)
        {
            InitializeComponent();
            ViewModel.AccountId = accountId;
        }
    }
}
