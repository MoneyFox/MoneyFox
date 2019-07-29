using System.Threading.Tasks;
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

            AddAccountGrid.GestureRecognizers.Add(new TapGestureRecognizer { Command = new Command(async () => await AddAccountGridClicked()) });
            AddExpenseGrid.GestureRecognizers.Add(new TapGestureRecognizer { Command = new Command(async () => await AddExpenseGridClicked()) });
            AddIncomeGrid.GestureRecognizers.Add(new TapGestureRecognizer { Command = new Command(async () => await AddIncomeGridClicked()) });
            AddTransferGrid.GestureRecognizers.Add(new TapGestureRecognizer { Command = new Command(async () => await AddTransferGridClicked()) });
        }

        private async Task AddAccountGridClicked()
        {
            await Navigation.PopPopupAsync();
            ViewModel?.GoToAddAccountCommand.Execute(null);
        }

        private async Task AddExpenseGridClicked()
        {
            await Navigation.PopPopupAsync();
            ViewModel?.GoToAddExpenseCommand.Execute(null);
        }

        private async Task AddIncomeGridClicked()
        {
            await Navigation.PopPopupAsync();
            ViewModel?.GoToAddIncomeCommand.Execute(null);
        }

        private async Task AddTransferGridClicked()
        {
            await Navigation.PopPopupAsync();
            ViewModel?.GoToAddTransferCommand.Execute(null);
        }
    }
}