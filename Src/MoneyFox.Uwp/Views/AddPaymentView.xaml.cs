using MoneyFox.Domain;
using MoneyFox.Application.Common;
using MoneyFox.Uwp.ViewModels;
using Windows.UI.Xaml.Controls;

namespace MoneyFox.Uwp.Views
{
    public sealed partial class AddPaymentView : ContentDialog
    {
        private AddPaymentViewModel ViewModel => DataContext as AddPaymentViewModel;

        public AddPaymentView(PaymentType paymentType)
        {
            InitializeComponent();

            ViewModel.PaymentType = paymentType;
            ViewModel.InitializeCommand.ExecuteAsync().FireAndForgetSafeAsync();
        }
    }
}
