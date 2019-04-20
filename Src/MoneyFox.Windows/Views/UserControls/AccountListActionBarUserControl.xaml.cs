using System.Reactive.Disposables;
using MoneyFox.ServiceLayer.ViewModels;
using ReactiveUI;

namespace MoneyFox.Windows.Views.UserControls
{
    public class MyAccountListActionBarUserControl : ReactiveUserControl<AccountListActionBarViewModel>{
    }

    public sealed partial class AccountListActionBarUserControl
    {
        public AccountListActionBarUserControl() {
            this.InitializeComponent();

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
            });
        }
    }
}
