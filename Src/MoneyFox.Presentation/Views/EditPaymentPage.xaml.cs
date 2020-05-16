using MoneyFox.Application.Common;
using MoneyFox.Application.Resources;
using MoneyFox.Presentation.ViewModels;
using System.Threading.Tasks;
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

            var cancelItem = new ToolbarItem
            {
                Command = new Command(async () => await Close()),
                Text = Strings.CancelLabel,
                Priority = -1,
                Order = ToolbarItemOrder.Primary
            };

            var saveItem = new ToolbarItem
                           {
                               Command = new Command(async() => await ViewModel.SaveCommand.ExecuteAsync()),
                               Text = Strings.SaveLabel,
                               Priority = 1,
                               Order = ToolbarItemOrder.Primary
                           };

            ToolbarItems.Add(cancelItem);
            ToolbarItems.Add(saveItem);

            ViewModel.PaymentId = paymentId;
            ViewModel.InitializeCommand.ExecuteAsync().FireAndForgetSafeAsync();
        }

        private async Task Close()
        {
            await Navigation.PopModalAsync();
        }
    }
}
