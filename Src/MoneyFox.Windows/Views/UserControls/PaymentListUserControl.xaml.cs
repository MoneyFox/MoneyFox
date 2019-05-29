using System.Reactive.Disposables;
using System.Reactive.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Input;
using MoneyFox.ServiceLayer.ViewModels;
using ReactiveUI;

namespace MoneyFox.Windows.Views.UserControls
{
    public class MyPaymentListUserControl : ReactiveUserControl<PaymentListViewModel> { }

    public sealed partial class PaymentListUserControl
    {
        public PaymentListUserControl()
        {
            InitializeComponent();

            this.WhenActivated(disposables =>
            {
                this.OneWayBind(ViewModel, vm => vm.Payments, v => v.PaymentCollectionViewSource.Source).DisposeWith(disposables);
                this.OneWayBind(ViewModel, vm => vm.Resources["NoPaymentMessage"], v => v.NoPaymentTextBlock.Text).DisposeWith(disposables);
                this.OneWayBind(ViewModel, vm => vm.HasNoPayments, v => v.NoPaymentTextBlock.Visibility).DisposeWith(disposables);

                PaymentListView
                    .Events()
                    .ItemClick.Select(x => x.ClickedItem as PaymentViewModel)
                    .InvokeCommand(ViewModel.EditPaymentCommand)
                    .DisposeWith(disposables);
            });
        }

        private void EditPaymentViewModel(object sender, RoutedEventArgs e)
        {
            var element = (FrameworkElement)sender;
            if (!(element.DataContext is PaymentViewModel payment))
            {
                return;
            }
            ViewModel.EditPaymentCommand.Execute(payment);
        }

        private void DeletePaymentViewModel(object sender, RoutedEventArgs e)
        {
            var element = (FrameworkElement)sender;
            if (!(element.DataContext is PaymentViewModel payment))
            {
                return;
            }
            ViewModel.DeletePaymentCommand.Execute(payment);
        }

        private void PaymentViewModelList_RightTapped(object sender, RightTappedRoutedEventArgs e)
        {
            var senderElement = sender as FrameworkElement;
            var flyoutBase = FlyoutBase.GetAttachedFlyout(senderElement) as MenuFlyout;

            flyoutBase?.ShowAt(senderElement, e.GetPosition(senderElement));
        }
    }
}
