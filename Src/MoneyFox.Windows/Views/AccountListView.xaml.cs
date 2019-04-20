using System.Reactive.Disposables;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
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

            this.WhenActivated(async disposables =>
            {
                await ViewModel.LoadAccounts();

                this.OneWayBind(ViewModel, vm => vm.Accounts, v => v.AccountsCollectionViewSource.Source)
                    .DisposeWith(disposables);

                this.BindCommand(ViewModel, vm => )

                this.OneWayBind(ViewModel, vm => vm.Resources["NoAccountsMessage"], v => v.NoAccountsTextBlock.Text)
                    .DisposeWith(disposables);

                this.OneWayBind(ViewModel, vm => vm.HasNoAccounts, v => v.NoAccountsTextBlock.Visibility)
                    .DisposeWith(disposables);
            });
        }

        object IViewFor.ViewModel
        {
            get => ViewModel;
            set => ViewModel = (AccountListViewModel)value;
        }

        public AccountListViewModel ViewModel { get; set; }
    }
}
