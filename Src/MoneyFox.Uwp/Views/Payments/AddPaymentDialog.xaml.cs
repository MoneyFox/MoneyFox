using MoneyFox.Domain;
using MoneyFox.Application.Common;
using MoneyFox.Uwp.ViewModels;

namespace MoneyFox.Uwp.Views.Payments
{
    public sealed partial class AddPaymentPage
    {
        public AddPaymentPage(PaymentType paymentType)
        {
            InitializeComponent();

            ViewModel.PaymentType = paymentType;
            ViewModel.InitializeCommand.ExecuteAsync().FireAndForgetSafeAsync();
        }

        public AddPaymentViewModel ViewModel => (AddPaymentViewModel)DataContext;
    }
}
