using System.Linq;
using MoneyFox.DataAccess.Repositories;
using MoneyFox.Foundation;
using MoneyFox.Foundation.Interfaces.ViewModels;
using MvvmCross.Core.ViewModels;
using MoneyFox.Service.DataServices;

namespace MoneyFox.Business.ViewModels
{
    /// <inheritdoc />
    public class AccountListViewActionViewModel : BaseViewModel//, IAccountListViewActionViewModel
    {
        private readonly IAccountService accountService;

        public AccountListViewActionViewModel(IAccountService accountService)
        {
            this.accountService = accountService;
        }

        //public MvxCommand GoToAddAccountCommand =>
        //        new MvxCommand(() => ShowViewModel<ModifyAccountViewModel>(new { accountId = 0 }));

        //public MvxCommand GoToAddIncomeCommand =>
        //        new MvxCommand(() => ShowViewModel<ModifyPaymentViewModel>(new { type = PaymentType.Income}));
        //public MvxCommand GoToAddExpenseCommand =>
        //        new MvxCommand(() => ShowViewModel<ModifyPaymentViewModel>(new { type = PaymentType.Expense }));
        //public MvxCommand GoToAddTransferCommand =>
        //        new MvxCommand(() => ShowViewModel<ModifyPaymentViewModel>(new { type = PaymentType.Transfer }));

        /// <summary>
        ///     Indicates if the transfer option is available or if it shall be hidden.
        /// </summary>
        public bool IsTransferAvailable => accountService.GetAccountCount().Result > 1;

        /// <summary>
        ///     Indicates if the button to add new income should be enabled.
        /// </summary>
        public bool IsAddIncomeAvailable => accountService.GetAccountCount().Result > 0;

        /// <summary>
        ///     Indicates if the button to add a new expense should be enabled.
        /// </summary>
        public bool IsAddExpenseAvailable => accountService.GetAccountCount().Result > 0;
    }
}
