using MoneyFox.ServiceLayer.ViewModels.Interfaces;
using Rg.Plugins.Popup.Extensions;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MoneyFox.Presentation.Dialogs
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class AddAccountAndPaymentPopup
	{
		public AddAccountAndPaymentPopup ()
		{
			InitializeComponent ();

            AddAccountGrid.GestureRecognizers.Add(new TapGestureRecognizer { Command = new Command(AddAccountGridClicked) });
            AddExpenseGrid.GestureRecognizers.Add(new TapGestureRecognizer { Command = new Command(AddExpenseGridClicked) });
            AddIncomeGrid.GestureRecognizers.Add(new TapGestureRecognizer { Command = new Command(AddIncomeGridClicked) });
            AddTransferGrid.GestureRecognizers.Add(new TapGestureRecognizer { Command = new Command(AddTransferGridClicked) });
        }

        private async void AddAccountGridClicked()
        {
            await Navigation.PopPopupAsync().ConfigureAwait(true);
            (BindingContext as IAccountListViewActionViewModel)?.GoToAddAccountCommand.ExecuteAsync();
        }

        private async void AddExpenseGridClicked()
        {
            await Navigation.PopPopupAsync().ConfigureAwait(true);
            (BindingContext as IAccountListViewActionViewModel)?.GoToAddExpenseCommand.ExecuteAsync();
        }

        private async void AddIncomeGridClicked()
        {
            await Navigation.PopPopupAsync().ConfigureAwait(true);
            (BindingContext as IAccountListViewActionViewModel)?.GoToAddIncomeCommand.ExecuteAsync();
        }

        private async void AddTransferGridClicked()
        {
            await Navigation.PopPopupAsync().ConfigureAwait(true);
            (BindingContext as IAccountListViewActionViewModel)?.GoToAddTransferCommand.ExecuteAsync();
        }
    }
}