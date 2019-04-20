using System.Linq;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using GenericServices;
using MoneyFox.Foundation;
using ReactiveUI;
using Splat;

namespace MoneyFox.ServiceLayer.ViewModels
{
    public class AccountListActionBarViewModel : ViewModelBase, ISupportsActivation
    {
        private int accountCount;

        private readonly IScreen hostScreen;
        private readonly ICrudServicesAsync crudServicesAsync;

        public AccountListActionBarViewModel(IScreen hostScreen, ICrudServicesAsync crudServicesAsync = null)
        {
            this.hostScreen = hostScreen;
            this.crudServicesAsync = crudServicesAsync ?? Locator.Current.GetService<ICrudServicesAsync>();

            Activator = new ViewModelActivator();
            this.WhenActivated(disposable =>
            {
                accountCount = this.crudServicesAsync.ReadManyNoTracked<AccountViewModel>().Count();

                var canExecutePayment = this.WhenAnyValue(x => x.accountCount, x => x >= 1);
                var canExecuteTransfer = this.WhenAnyValue(x => x.accountCount, x => x >= 2);

                GoToAddIncomeCommand = ReactiveCommand.Create(() => GoToAddPayment(PaymentType.Income), canExecutePayment).DisposeWith(disposable);
                GoToAddExpenseCommand = ReactiveCommand.Create(() => GoToAddPayment(PaymentType.Expense), canExecutePayment).DisposeWith(disposable);
                GoToAddTransferCommand = ReactiveCommand.Create(() => GoToAddPayment(PaymentType.Transfer), canExecuteTransfer).DisposeWith(disposable);
                GoToAddAccountCommand = ReactiveCommand.Create(GoToAddAccount).DisposeWith(disposable);
            });
        }

        public ViewModelActivator Activator { get; }
        public ReactiveCommand<Unit, Unit> GoToAddIncomeCommand { get; set; }
        public ReactiveCommand<Unit, Unit> GoToAddExpenseCommand { get; set; }
        public ReactiveCommand<Unit, Unit> GoToAddTransferCommand { get; set; }
        public ReactiveCommand<Unit, Unit> GoToAddAccountCommand { get; set; }

        private Unit GoToAddPayment(PaymentType type)
        {
            //hostScreen.Router.Navigate.Execute(new AddPaymentViewModel(hostScreen));
            return new Unit();
        }
        private Unit GoToAddAccount()
        {
            hostScreen.Router.Navigate.Execute(new AddAccountViewModel(hostScreen));
            return new Unit();
        }
    }
}
