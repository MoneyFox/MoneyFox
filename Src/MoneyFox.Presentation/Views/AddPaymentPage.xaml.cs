using MoneyFox.Application.Common;
using MoneyFox.Application.Resources;
using MoneyFox.Domain;
using MoneyFox.Presentation.ViewModels;
using System.Threading.Tasks;
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

            var cancelItem = new ToolbarItem
            {
                Command = new Command(async () => await Close()),
                Text = Strings.CancelLabel,
                Priority = -1,
                Order = ToolbarItemOrder.Primary
            };

            var saveItem = new ToolbarItem
            {
                Command = new Command(async () => await ViewModel.SaveCommand.ExecuteAsync()),
                Text = Strings.SaveLabel,
                Priority = 1,
                Order = ToolbarItemOrder.Primary
            };

            ToolbarItems.Add(cancelItem);
            ToolbarItems.Add(saveItem);

            ViewModel.PaymentType = paymentType;
            ViewModel.InitializeCommand.ExecuteAsync().FireAndForgetSafeAsync();
        }

        private async Task Close()
        {
            await Navigation.PopModalAsync();
        }
    }
}
