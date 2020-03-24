using MoneyFox.Application.Common;
using MoneyFox.Application.Resources;
using MoneyFox.Domain;
using MoneyFox.Presentation.ViewModels;
using MoneyFox.Ui.Shared.Utilities;
using Xamarin.Forms;

namespace MoneyFox.Presentation.Views
{
    public partial class AddPaymentPage
    {
        private AddPaymentViewModel ViewModel => BindingContext as AddPaymentViewModel;

        public AddPaymentPage(PaymentType paymentType)
        {
            InitializeComponent();
            BindingContext = ViewModelLocator.AddPaymentVm;

            var saveItem = new ToolbarItem
                           {
                               Command = new Command(async() => await ViewModel.SaveCommand.ExecuteAsync()),
                               Text = Strings.SaveLabel,
                               Priority = 0,
                               Order = ToolbarItemOrder.Primary
                           };

            ToolbarItems.Add(saveItem);

            ViewModel.PaymentType = paymentType;
            ViewModel.InitializeCommand.ExecuteAsync().FireAndForgetSafeAsync();
        }
    }
}
