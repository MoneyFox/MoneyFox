using MoneyFox.Foundation;
using MoneyFox.Presentation.Utilities;
using MoneyFox.Presentation.ViewModels;

namespace MoneyFox.Presentation.Views
{
	public partial class AddPaymentPage
	{
        private AddPaymentViewModel ViewModel => BindingContext as AddPaymentViewModel;

        public AddPaymentPage(PaymentType paymentType)
		{
			InitializeComponent ();
            BindingContext = ViewModelLocator.AddPaymentVm;

            ViewModel.SelectedPayment.Type = paymentType;
            ViewModel.InitializeCommand.ExecuteAsync().FireAndForgetSafeAsync();
        }
    }
}