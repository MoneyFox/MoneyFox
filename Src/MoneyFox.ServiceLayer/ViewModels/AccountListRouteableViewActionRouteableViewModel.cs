using System.Linq;
using GenericServices;
using ReactiveUI;

namespace MoneyFox.ServiceLayer.ViewModels
{
    public class AccountListRouteableViewActionRouteableViewModel : RouteableViewModelBase
    {
        private readonly ICrudServicesAsync crudServices;

        public AccountListRouteableViewActionRouteableViewModel(IScreen hostScreen, ICrudServicesAsync crudServices)
        {
            HostScreen = hostScreen;
            this.crudServices = crudServices;
        }

        public override string UrlPathSegment => "AccountListActions";
        public override IScreen HostScreen { get; }

        /// <inheritdoc />
        //public ReactiveCommand<Unit, Unit> GoToAddAccountCommand 
        //    => ReactiveCommand.Create<Unit, Unit>(() => HostScreen.Router.Navigate.Execute(new AddAccountRouteableViewModel(this)));

        /// <inheritdoc />
        //public MvxAsyncCommand GoToAddIncomeCommand =>
        //        new MvxAsyncCommand(async () => await navigationService.Navigate<AddPaymentViewModel, ModifyPaymentParameter>(new ModifyPaymentParameter(PaymentType.Income)));

        ///// <inheritdoc />
        //public MvxAsyncCommand GoToAddExpenseCommand =>
        //    new MvxAsyncCommand(async () => await navigationService.Navigate<AddPaymentViewModel, ModifyPaymentParameter>(new ModifyPaymentParameter(PaymentType.Expense)));

        ///// <inheritdoc />
        //public MvxAsyncCommand GoToAddTransferCommand =>
        //    new MvxAsyncCommand(async () => await navigationService.Navigate<AddPaymentViewModel, ModifyPaymentParameter>(new ModifyPaymentParameter(PaymentType.Transfer)));

        /// <summary>
        ///     Indicates if the transfer option is available or if it shall be hidden.
        /// </summary>
        public bool IsTransferAvailable => crudServices.ReadManyNoTracked<AccountViewModel>().Count() >= 2;

        /// <summary>
        ///     Indicates if the button to add new income should be enabled.
        /// </summary>
        public bool IsAddIncomeAvailable => crudServices.ReadManyNoTracked<AccountViewModel>().Any();

        /// <summary>
        ///     Indicates if the button to add a new expense should be enabled.
        /// </summary>
        public bool IsAddExpenseAvailable => crudServices.ReadManyNoTracked<AccountViewModel>().Any();
    }
}
