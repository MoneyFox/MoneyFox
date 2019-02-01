using System.Linq;
using GenericServices;
using MoneyFox.Foundation;
using MoneyFox.ServiceLayer.Parameters;
using MoneyFox.ServiceLayer.ViewModels.Interfaces;
using MvvmCross.Commands;
using MvvmCross.Logging;
using MvvmCross.Navigation;

namespace MoneyFox.ServiceLayer.ViewModels
{
    public class AccountListViewActionViewModel : BaseNavigationViewModel, IAccountListViewActionViewModel
    {
        private readonly ICrudServicesAsync crudServices;
        private readonly IMvxNavigationService navigationService;

        public AccountListViewActionViewModel(ICrudServicesAsync crudServices,
                                              IMvxLogProvider logProvider,
                                              IMvxNavigationService navigationService) : base(logProvider, navigationService)
        {
            this.crudServices = crudServices;
            this.navigationService = navigationService;
        }
        
        /// <inheritdoc />
        public MvxAsyncCommand GoToAddAccountCommand =>
                new MvxAsyncCommand(async () => await navigationService.Navigate<AddAccountViewModel, ModifyAccountParameter>(new ModifyAccountParameter()));

        /// <inheritdoc />
        public MvxAsyncCommand GoToAddIncomeCommand =>
                new MvxAsyncCommand(async () => await navigationService.Navigate<AddPaymentViewModel, ModifyPaymentParameter>(new ModifyPaymentParameter(PaymentType.Income)));

        /// <inheritdoc />
        public MvxAsyncCommand GoToAddExpenseCommand =>
            new MvxAsyncCommand(async () => await navigationService.Navigate<AddPaymentViewModel, ModifyPaymentParameter>(new ModifyPaymentParameter(PaymentType.Expense)));

        /// <inheritdoc />
        public MvxAsyncCommand GoToAddTransferCommand =>
            new MvxAsyncCommand(async () => await navigationService.Navigate<AddPaymentViewModel, ModifyPaymentParameter>(new ModifyPaymentParameter(PaymentType.Transfer)));

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
