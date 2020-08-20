using MoneyFox.Application.Resources;
using MoneyFox.ViewModels.Payments;
using Xamarin.Forms;

namespace MoneyFox.Views.Payments
{
    public partial class EditPaymentPage
    {
        private EditPaymentViewModel ViewModel => (EditPaymentViewModel) BindingContext;

        private int paymentId;

        public EditPaymentPage(int paymentId)
        {
            InitializeComponent();
            BindingContext = ViewModelLocator.EditPaymentViewModel;

            this.paymentId = paymentId;

            var cancelItem = new ToolbarItem
            {
                Command = new Command(async () => await Navigation.PopModalAsync()),
                Text = Strings.CancelLabel,
                Priority = -1,
                Order = ToolbarItemOrder.Primary
            };

            var saveItem = new ToolbarItem
            {
                Command = new Command(() => ViewModel.SaveCommand.Execute(null)),
                Text = Strings.SaveLabel,
                Priority = 1,
                Order = ToolbarItemOrder.Primary
            };

            ToolbarItems.Add(cancelItem);
            ToolbarItems.Add(saveItem);
        }

        protected override async void OnAppearing() => await ViewModel.InitializeAsync(paymentId);
    }
}
