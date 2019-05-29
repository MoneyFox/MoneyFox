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
                PaymentListUserControl.ViewModel = ViewModel;

                this.OneWayBind(ViewModel, vm => vm.Title, v => TitlePage.Text).DisposeWith(disposables);
                this.OneWayBind(ViewModel, vm => vm.Resources["FilterLabel"], v => v.FilterButton.Label).DisposeWith(disposables);
                this.OneWayBind(ViewModel, vm => vm.Resources["AddIncomeLabel"], v => v.AddIncomeButton.Label).DisposeWith(disposables);
                this.OneWayBind(ViewModel, vm => vm.Resources["AddExpenseLabel"], v => v.AddExpenseButton.Label).DisposeWith(disposables);
                this.OneWayBind(ViewModel, vm => vm.Resources["AddTransferLabel"], v => v.AddTransferButton.Label).DisposeWith(disposables);
                this.OneWayBind(ViewModel, vm => vm.Resources["DeleteAccountLabel"], v => v.DeleteAccountButton.Label).DisposeWith(disposables);
            });
        }
        
        private void AppBarToggleButton_Click(object sender, RoutedEventArgs e)
        {
            FlyoutBase.ShowAttachedFlyout((FrameworkElement)sender);
        }
    }
}
