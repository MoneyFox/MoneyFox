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

        public AccountListView()
        {
            this.InitializeComponent();

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
            set => ViewModel = (AccountListViewModel) value;
        }

        public AccountListViewModel ViewModel {
            get => (AccountListViewModel)GetValue(ViewModelProperty);
            set => SetValue(ViewModelProperty, value);
        }

        private void AccountList_RightTapped(object sender, RightTappedRoutedEventArgs e)
        {
            var senderElement = sender as FrameworkElement;
            var flyoutBase = FlyoutBase.GetAttachedFlyout(senderElement) as MenuFlyout;

            flyoutBase?.ShowAt(senderElement, e.GetPosition(senderElement));
        }

        private void EditFlyoutClicked(object sender, RoutedEventArgs e)
        {
            var element = (FrameworkElement) sender;
            if (!(element.DataContext is AccountViewModel account))
            {
                return;
            }

            ViewModel.EditAccountCommand.Execute(account);
        }

        private void DeleteFlyoutClicked(object sender, RoutedEventArgs e)
        {
            var element = (FrameworkElement) sender;
            if (!(element.DataContext is AccountViewModel account))
            {
                return;
            }

            ViewModel.DeleteAccountCommand.Execute(account);
        }
    }
}