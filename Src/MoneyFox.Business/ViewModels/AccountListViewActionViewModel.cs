using System.Linq;
using MoneyFox.Foundation;
using MoneyFox.Foundation.Interfaces.Repositories;
using MoneyFox.Foundation.Interfaces.ViewModels;
using MvvmCross.Core.ViewModels;

namespace MoneyFox.Business.ViewModels
{
    /// <summary>
    ///     Represents the Actions for a view.
    ///     On Windows this is a normaly in the app bar. 
    ///     On Android for example in a floating action button.
    /// </summary>
    public class AccountListViewActionViewModel : BaseViewModel, IAccountListViewActionViewModel
    {
        private readonly IAccountRepository accountRepository;

        public AccountListViewActionViewModel(IAccountRepository accountRepository)
        {
            this.accountRepository = accountRepository;
        }

        public MvxCommand GoToAddAccountCommand => 
                new MvxCommand(() => ShowViewModel<ModifyAccountViewModel>(new { isEdit = false, selectedAccountId = 0 }));

        public MvxCommand GoToAddIncomeCommand =>
                new MvxCommand(() => ShowViewModel<ModifyPaymentViewModel>(new { type = PaymentType.Income}));
        public MvxCommand GoToAddExpenseCommand =>
                new MvxCommand(() => ShowViewModel<ModifyPaymentViewModel>(new { type = PaymentType.Expense }));
        public MvxCommand GoToAddTransferCommand =>
                new MvxCommand(() => ShowViewModel<ModifyPaymentViewModel>(new { type = PaymentType.Transfer }));

        /// <summary>
        ///     Indicates if the transfer option is available or if it shall be hidden.
        /// </summary>
        public bool IsTransferAvailable => accountRepository.GetList().Count() > 1;

        /// <summary>
        ///     Indicates if the button to add new income should be enabled.
        /// </summary>
        public bool IsAddIncomeAvailable => accountRepository.GetList().Any();

        /// <summary>
        ///     Indicates if the button to add a new expense should be enabled.
        /// </summary>
        public bool IsAddExpenseAvailable => accountRepository.GetList().Any();
    }
}
