using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Input;
using MoneyFox.Shared.Model;
using MoneyFox.Shared.ViewModels;
using MvvmCross.Platform;
using Windows.UI.Xaml.Controls;

namespace MoneyFox.Windows.Views.UserControls {
    public partial class PaymentListUserControl {
        public PaymentListUserControl() {
            InitializeComponent();
        }

        private void EditPaymentViewModel(object sender, RoutedEventArgs e) {
            var element = (FrameworkElement) sender;
            var payment = element.DataContext as Payment;
            if (payment == null) {
                return;
            }
            var viewmodel = DataContext as PaymentListViewModel;

            viewmodel?.EditCommand.Execute(payment);
        }

        private void DeletePaymentViewModel(object sender, RoutedEventArgs e) {
            var element = (FrameworkElement) sender;
            var payment = element.DataContext as Payment;
            if (payment == null) {
                return;
            }
            (DataContext as PaymentListViewModel)?.DeletePaymentCommand.Execute(payment);
        }

        private void PaymentViewModelList_Holding(object sender, HoldingRoutedEventArgs e) {
            var senderElement = sender as FrameworkElement;
            var flyoutBase = FlyoutBase.GetAttachedFlyout(senderElement) as MenuFlyout;

            flyoutBase.ShowAt(senderElement, e.GetPosition(senderElement));
        }

        private void PaymentViewModelList_RightTapped(object sender, RightTappedRoutedEventArgs e) {
            var senderElement = sender as FrameworkElement;
            var flyoutBase = FlyoutBase.GetAttachedFlyout(senderElement) as MenuFlyout;

            flyoutBase.ShowAt(senderElement, e.GetPosition(senderElement));
        }
    }
}