using MoneyFox.Application.Common;
using MoneyFox.Uwp.ViewModels;
using Windows.UI.Xaml.Controls;

namespace MoneyFox.Uwp.Views.Payments
{
    public sealed partial class EditPaymentView : ContentDialog
    {
        private EditPaymentViewModel ViewModel => (EditPaymentViewModel) DataContext;

        public EditPaymentView(int paymentId)
        {
            InitializeComponent();

            ViewModel.PaymentId = paymentId;
            ViewModel.InitializeCommand.ExecuteAsync().FireAndForgetSafeAsync();
        }

        private void Dismiss(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            Hide();
        }
    }
}
