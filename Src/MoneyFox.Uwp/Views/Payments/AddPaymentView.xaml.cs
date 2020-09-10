using MoneyFox.Domain;
using MoneyFox.Application.Common;
using MoneyFox.Uwp.ViewModels;

namespace MoneyFox.Uwp.Views.Payments
{
    public sealed partial class AddPaymentView
    {
        public AddPaymentView(PaymentType paymentType)
        {
            InitializeComponent();

            ViewModel.PaymentType = paymentType;
            ViewModel.InitializeCommand.ExecuteAsync().FireAndForgetSafeAsync();
        }

        public AddPaymentViewModel ViewModel => (AddPaymentViewModel)DataContext;
    }
}
