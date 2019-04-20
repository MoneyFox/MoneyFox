using System.Reactive.Disposables;
using MoneyFox.ServiceLayer.ViewModels;
using ReactiveUI;
using Splat;

namespace MoneyFox.Windows.Views.UserControls
{
    public class MyAccountListActionBarUserControl : ReactiveUserControl<AccountListActionBarViewModel>{
    }

    public sealed partial class AccountListActionBarUserControl
    {
        public AccountListActionBarUserControl() {
            this.InitializeComponent();

            ViewModel = Locator.Current.GetService<AccountListActionBarViewModel>();

            this.WhenActivated(disposables =>
            {
                this.BindCommand(ViewModel, vm => vm.GoToAddIncomeCommand, v => v.AddIncomeButton.Command)
                    .DisposeWith(disposables);
                this.BindCommand(ViewModel, vm => vm.GoToAddExpenseCommand, v => v.AddExpenseButton.Command)
                    .DisposeWith(disposables);
                this.BindCommand(ViewModel, vm => vm.GoToAddTransferCommand, v => v.AddTransferButton.Command)
                    .DisposeWith(disposables);
                this.BindCommand(ViewModel, vm => vm.GoToAddAccountCommand, v => v.AddAccountButton.Command)
                    .DisposeWith(disposables);

                this.OneWayBind(ViewModel, vm => vm.Resources["AddIncomeTitle"], v => v.AddIncomeButton.Label);
                this.OneWayBind(ViewModel, vm => vm.Resources["AddExpenseTitle"], v => v.AddExpenseButton.Label);
                this.OneWayBind(ViewModel, vm => vm.Resources["AddTransferTitle"], v => v.AddTransferButton.Label);
                this.OneWayBind(ViewModel, vm => vm.Resources["AddAccountTitle"], v => v.AddAccountButton.Label);
            });
        }
    }
}
