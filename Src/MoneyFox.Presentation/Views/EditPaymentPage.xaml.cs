using MoneyFox.Foundation.Resources;
using MoneyFox.Presentation.Utilities;
using MoneyFox.Presentation.ViewModels;
using Xamarin.Forms;

namespace MoneyFox.Presentation.Views
{
	public partial class EditPaymentPage
    {
        private EditPaymentViewModel ViewModel => BindingContext as EditPaymentViewModel;

        public EditPaymentPage(int paymentId)
		{
			InitializeComponent();
            BindingContext = ViewModelLocator.EditPaymentVm;

            ToolbarItems.Add(new ToolbarItem
            {
                Command = new Command(() => ViewModel?.SaveCommand.ExecuteAsync().FireAndForgetSafeAsync()),
                Text = Strings.SavePaymentLabel,
                Priority = 0,
                Order = ToolbarItemOrder.Primary,
                IconImageSource = "ic_save.png"
            });

            ToolbarItems.Add(new ToolbarItem
            {
                Command = new Command(() => ViewModel?.DeleteCommand.ExecuteAsync().FireAndForgetSafeAsync()),
                Text = Strings.DeleteLabel,
                Priority = 1,
                Order = ToolbarItemOrder.Secondary
            });

            ViewModel.PaymentId = paymentId;
            ViewModel.InitializeCommand.ExecuteAsync().FireAndForgetSafeAsync();
        }
    }
}