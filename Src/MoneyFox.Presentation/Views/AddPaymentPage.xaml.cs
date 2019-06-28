using MoneyFox.Foundation;
using MoneyFox.Foundation.Resources;
using MoneyFox.Presentation.Utilities;
using MoneyFox.Presentation.ViewModels;
using Xamarin.Forms;

namespace MoneyFox.Presentation.Views
{
	public partial class AddPaymentPage
	{
        private AddPaymentViewModel ViewModel => BindingContext as AddPaymentViewModel;

        public AddPaymentPage(PaymentType paymentType)
		{
			InitializeComponent ();
            BindingContext = ViewModelLocator.AddPaymentVm;

            ToolbarItems.Add(new ToolbarItem
            {
                Command = new Command(async () => await ViewModel?.SaveCommand.ExecuteAsync()),
                Text = Strings.SavePaymentLabel,
                Priority = 0,
                Order = ToolbarItemOrder.Primary,
                IconImageSource = "ic_save.png"
            });

            ViewModel.SelectedPayment.Type = paymentType;
            ViewModel.InitializeCommand.ExecuteAsync().FireAndForgetSafeAsync();
        }
    }
}