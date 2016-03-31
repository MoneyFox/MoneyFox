using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Input;
using Microsoft.Practices.ServiceLocation;
using MoneyFox.Core.Model;
using MoneyFox.Core.ViewModels;

namespace MoneyFox.Windows.Views.UserControls
{
    public partial class PaymentViewModelListUserControl
    {
        public PaymentViewModelListUserControl()
        {
            InitializeComponent();
            DataContext = ServiceLocator.Current.GetInstance<PaymentListViewModel>();
        }

        private void EditPaymentViewModel(object sender, RoutedEventArgs e)
        {
            var element = (FrameworkElement) sender;
            var paymentViewModel = element.DataContext as PaymentViewModel;
            if (paymentViewModel == null)
            {
                return;
            }
            var viewmodel = DataContext as PaymentListViewModel;

            viewmodel?.EditCommand.Execute(paymentViewModel);
        }

        private void DeletePaymentViewModel(object sender, RoutedEventArgs e)
        {
            var element = (FrameworkElement) sender;
            var paymentViewModel = element.DataContext as PaymentViewModel;
            if (paymentViewModel == null)
            {
                return;
            }
            (DataContext as PaymentListViewModel)?.DeletePaymentViewModelCommand.Execute(paymentViewModel);
        }

        private void PaymentViewModelList_Holding(object sender, HoldingRoutedEventArgs e)
        {
            var senderElement = sender as FrameworkElement;
            var flyoutBase = FlyoutBase.GetAttachedFlyout(senderElement);

            flyoutBase.ShowAt(senderElement);
        }

        private void PaymentViewModelList_RightTapped(object sender, RightTappedRoutedEventArgs e)
        {
            var senderElement = sender as FrameworkElement;
            var flyoutBase = FlyoutBase.GetAttachedFlyout(senderElement);

            flyoutBase.ShowAt(senderElement);
        }
    }
}