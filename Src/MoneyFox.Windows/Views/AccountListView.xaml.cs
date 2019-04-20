using System.Reactive.Disposables;
using System.Reactive.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Input;
using MoneyFox.ServiceLayer.ViewModels;
using ReactiveUI;
using Splat;

namespace MoneyFox.Windows.Views
{
    public sealed partial class AccountListView : IViewFor<AccountListViewModel>
    {
        public static readonly DependencyProperty ViewModelProperty = DependencyProperty
            .Register(nameof(ViewModel), typeof(AccountListViewModel), typeof(AccountListView), null);


        public AccountListView() {
            this.InitializeComponent();

            ViewModel = Locator.Current.GetService<AccountListViewModel>();

            this.WhenActivated(disposables =>
            {
                this.OneWayBind(ViewModel, vm => vm.Accounts, v => v.AccountsCollectionViewSource.Source)
                    .DisposeWith(disposables);

                this.OneWayBind(ViewModel, vm => vm.Resources["NoAccountsMessage"], v => v.NoAccountsTextBlock.Text)
                    .DisposeWith(disposables);

                this.OneWayBind(ViewModel, vm => vm.HasNoAccounts, v => v.NoAccountsTextBlock.Visibility)
                    .DisposeWith(disposables);

                AccountList
                    .Events()
                    .ItemClick.Select(x => x.ClickedItem as AccountViewModel)
                    .InvokeCommand(ViewModel.GoToPaymentViewCommand)
                    .DisposeWith(disposables);
            });
        }

        object IViewFor.ViewModel
        {
            get => ViewModel;
            set => ViewModel = (AccountListViewModel)value;
        }

        public AccountListViewModel ViewModel { get; set; }

        private void AccountList_RightTapped(object sender, RightTappedRoutedEventArgs e)
        {
            var senderElement = sender as FrameworkElement;
            var flyoutBase = FlyoutBase.GetAttachedFlyout(senderElement) as MenuFlyout;

            flyoutBase?.ShowAt(senderElement, e.GetPosition(senderElement));
        }
    }
}
