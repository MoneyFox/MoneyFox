using Rg.Plugins.Popup.Extensions;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MoneyFox.Dialogs
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class AddPaymentDialog
    {
		public AddPaymentDialog()
		{
			InitializeComponent ();

		    AddExpenseGrid.GestureRecognizers.Add(new TapGestureRecognizer {Command = new Command(AddExpenseGridClicked) });
		    AddIncomeGrid.GestureRecognizers.Add(new TapGestureRecognizer {Command = new Command(AddIncomeGridClicked) });
		    AddTransferGrid.GestureRecognizers.Add(new TapGestureRecognizer {Command = new Command(AddTransferGridClicked) });
        }
        
	    private async void AddExpenseGridClicked()
	    {
	        await Navigation.PopPopupAsync();
            //(BindingContext as IPaymentListViewActionViewModel)?.GoToAddExpenseCommand.ExecuteAsync();
	    }

	    private async void AddIncomeGridClicked()
	    {
	        await Navigation.PopPopupAsync();
            //(BindingContext as IPaymentListViewActionViewModel)?.GoToAddIncomeCommand.ExecuteAsync();
	    }

	    private async void AddTransferGridClicked()
	    {
	        await Navigation.PopPopupAsync();
            //(BindingContext as IPaymentListViewActionViewModel)?.GoToAddTransferCommand.ExecuteAsync();
	    }
    }
}