using MoneyFox.Foundation;
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

            AddExpenseGrid.GestureRecognizers.Add(new TapGestureRecognizer { Command = new Command(AddExpenseGridClicked) });
            AddIncomeGrid.GestureRecognizers.Add(new TapGestureRecognizer { Command = new Command(AddIncomeGridClicked) });
            AddTransferGrid.GestureRecognizers.Add(new TapGestureRecognizer { Command = new Command(AddTransferGridClicked) });
        }

        private async void AddExpenseGridClicked()
        {
            await Navigation.PopPopupAsync();
            (BindingContext as IPaymentListViewActionViewModel)?.GoToAddExpenseCommand.Execute(PaymentType.Expense);
        }

        private async void AddIncomeGridClicked()
        {
            await Navigation.PopPopupAsync();
            (BindingContext as IPaymentListViewActionViewModel)?.GoToAddIncomeCommand.Execute(PaymentType.Income);
        }

        private async void AddTransferGridClicked()
        {
            await Navigation.PopPopupAsync();
            (BindingContext as IPaymentListViewActionViewModel)?.GoToAddTransferCommand.Execute(PaymentType.Transfer);
        }
    }
}