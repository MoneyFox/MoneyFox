using MoneyFox.Application.Common;
using MoneyFox.Application.Resources;
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

            var saveItem = new ToolbarItem
                           {
                               Command = new Command(async() => await ViewModel.SaveCommand.ExecuteAsync()),
                               Text = Strings.SaveLabel,
                               Priority = 0,
                               Order = ToolbarItemOrder.Primary
                           };

            ToolbarItems.Add(saveItem);

            ViewModel.PaymentId = paymentId;
            ViewModel.InitializeCommand.ExecuteAsync().FireAndForgetSafeAsync();
        }
    }
}
