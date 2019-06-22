using MoneyFox.Foundation;
using MoneyFox.Presentation.ViewModels.Interfaces;
using Rg.Plugins.Popup.Extensions;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MoneyFox.Presentation.Dialogs
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class AddAccountAndPaymentPopup
    {
        private IAccountListViewActionViewModel ViewModel => BindingContext as IAccountListViewActionViewModel;

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
            await Navigation.PopPopupAsync();
            ViewModel?.GoToAddAccountCommand.Execute(null);
        }

        private async void AddExpenseGridClicked()
        {
            await Navigation.PopPopupAsync();
            ViewModel?.GoToAddExpenseCommand.Execute(PaymentType.Expense);
        }

        private async void AddIncomeGridClicked()
        {
            await Navigation.PopPopupAsync();
            ViewModel?.GoToAddIncomeCommand.Execute(PaymentType.Income);
        }

        private async void AddTransferGridClicked()
        {
            await Navigation.PopPopupAsync();
            ViewModel?.GoToAddTransferCommand.Execute(PaymentType.Transfer);
        }
    }
}