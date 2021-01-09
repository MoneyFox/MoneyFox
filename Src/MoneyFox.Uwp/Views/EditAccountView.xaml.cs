using MoneyFox.Uwp.ViewModels;
using Windows.UI.Xaml.Controls;

#nullable enable
namespace MoneyFox.Uwp.Views
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
