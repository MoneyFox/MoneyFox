using System.Reactive.Disposables;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls.Primitives;
using MoneyFox.ServiceLayer.ViewModels;
using ReactiveUI;

namespace MoneyFox.Windows.Views
{
    public class MyPaymentListView : ReactiveView<PaymentListViewModel> { }


    public sealed partial class PaymentListView
    {
        public PaymentListView()
        {
            InitializeComponent();

            this.WhenActivated(disposables =>
            {
                this.OneWayBind(ViewModel, vm => vm.Resources["FilterLabel"], v => v.FilterButton).DisposeWith(disposables);
                this.OneWayBind(ViewModel, vm => vm.Resources["AddIncomeLabel"], v => v.AddIncomeButton).DisposeWith(disposables);
                this.OneWayBind(ViewModel, vm => vm.Resources["AddExpenseLabel"], v => v.AddExpenseButton).DisposeWith(disposables);
                this.OneWayBind(ViewModel, vm => vm.Resources["AddTransferLabel"], v => v.AddTransferButton).DisposeWith(disposables);
                this.OneWayBind(ViewModel, vm => vm.Resources["DeleteAccountLabel"], v => v.DeleteAccountButton).DisposeWith(disposables);
            });
        }
        
        private void AppBarToggleButton_Click(object sender, RoutedEventArgs e)
        {
            FlyoutBase.ShowAttachedFlyout((FrameworkElement)sender);
        }
    }
}
