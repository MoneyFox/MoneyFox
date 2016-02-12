using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Input;
using MoneyManager.Core.ViewModels;
using MoneyManager.Foundation.Model;
using MvvmCross.Platform;

namespace MoneyManager.Windows.Views.UserControls
{
    public partial class PaymentListUserControl
    {
        public PaymentListUserControl()
        {
            InitializeComponent();
            DataContext = Mvx.Resolve<PaymentListViewModel>();
        }

        private void EditPayment(object sender, RoutedEventArgs e)
        {
            var element = (FrameworkElement) sender;
            var payment = element.DataContext as Payment;
            if (payment == null)
            {
                return;
            }
            var viewmodel = DataContext as PaymentListViewModel;

            viewmodel?.EditCommand.Execute(payment);
        }

        private void DeletePayment(object sender, RoutedEventArgs e)
        {
            var element = (FrameworkElement) sender;
            var payment = element.DataContext as Payment;
            if (payment == null)
            {
                return;
            }
            (DataContext as PaymentListViewModel)?.DeletePaymentCommand.Execute(payment);
        }

        private void PaymentList_Holding(object sender, HoldingRoutedEventArgs e)
        {
            var senderElement = sender as FrameworkElement;
            var flyoutBase = FlyoutBase.GetAttachedFlyout(senderElement);

            flyoutBase.ShowAt(senderElement);
        }

        private void PaymentList_RightTapped(object sender, RightTappedRoutedEventArgs e)
        {
            var senderElement = sender as FrameworkElement;
            var flyoutBase = FlyoutBase.GetAttachedFlyout(senderElement);

            flyoutBase.ShowAt(senderElement);
        }
    }
}