using MoneyFox.Presentation.Utilities;
using MoneyFox.Presentation.ViewModels;

namespace MoneyFox.Presentation.Views
{
	public partial class EditAccountPage
    {
        private EditAccountViewModel ViewModel => BindingContext as EditAccountViewModel;

        public EditAccountPage(int accountId)
		{
			InitializeComponent();
            BindingContext = ViewModelLocator.EditAccountVm;

            ViewModel.AccountId = accountId;
            ViewModel.InitializeCommand.ExecuteAsync().FireAndForgetSafeAsync();
        }
	}
}
