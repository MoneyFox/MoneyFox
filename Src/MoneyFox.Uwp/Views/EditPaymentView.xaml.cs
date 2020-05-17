using MoneyFox.Application.Common;
using MoneyFox.Uwp.ViewModels;
using Windows.UI.Xaml.Controls;

namespace MoneyFox.Uwp.Views
{
    public sealed partial class EditPaymentView : ContentDialog
    {
        private EditPaymentViewModel ViewModel => DataContext as EditPaymentViewModel;

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
