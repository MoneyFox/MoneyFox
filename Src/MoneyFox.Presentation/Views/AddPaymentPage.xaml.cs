using MoneyFox.Foundation;
using MoneyFox.Foundation.Resources;
using MoneyFox.Presentation.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MoneyFox.Presentation.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class AddPaymentPage
	{
        private AddPaymentViewModel ViewModel => BindingContext as AddPaymentViewModel;

        public AddPaymentPage(PaymentType paymentType)
		{
			InitializeComponent ();
            BindingContext = ViewModelLocator.AddPaymentVm;

            ToolbarItems.Add(new ToolbarItem
            {
                Command = new Command(() => ViewModel?.SaveCommand.Execute(null)),
                Text = Strings.SavePaymentLabel,
                Priority = 0,
                Order = ToolbarItemOrder.Primary,
                Icon = "ic_save.png"
            });

            ViewModel.PaymentType = paymentType;
            ViewModel.InitializeCommand.Execute(null);
        }
    }
}