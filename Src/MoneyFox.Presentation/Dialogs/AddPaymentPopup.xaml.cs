using System.Threading.Tasks;
using MoneyFox.Domain;
using MoneyFox.Presentation.ViewModels.Interfaces;
using Rg.Plugins.Popup.Extensions;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MoneyFox.Presentation.Dialogs
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class AddPaymentPopup
	{
		public AddPaymentPopup ()
		{
			InitializeComponent ();

            AddExpenseGrid.GestureRecognizers.Add(new TapGestureRecognizer { Command = new Command(async () => await AddExpenseGridClicked()) });
            AddIncomeGrid.GestureRecognizers.Add(new TapGestureRecognizer { Command = new Command(async () => await AddIncomeGridClicked()) });
            AddTransferGrid.GestureRecognizers.Add(new TapGestureRecognizer { Command = new Command(async () => await AddTransferGridClicked()) });
        }

        private async Task AddExpenseGridClicked()
        {
            await Navigation.PopPopupAsync();
            (BindingContext as IPaymentListViewActionViewModel)?.GoToAddExpenseCommand.Execute(null);
        }

        private async Task AddIncomeGridClicked()
        {
            await Navigation.PopPopupAsync();
            (BindingContext as IPaymentListViewActionViewModel)?.GoToAddIncomeCommand.Execute(null);
        }

        private async Task AddTransferGridClicked()
        {
            await Navigation.PopPopupAsync();
            (BindingContext as IPaymentListViewActionViewModel)?.GoToAddTransferCommand.Execute(null);
        }
    }
}