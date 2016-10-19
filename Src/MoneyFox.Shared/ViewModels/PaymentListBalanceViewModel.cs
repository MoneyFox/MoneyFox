using MoneyFox.Foundation.Interfaces;
using MoneyFox.Foundation.Interfaces.Repositories;
using MoneyFox.Shared.Interfaces;
using MoneyFox.Shared.Interfaces.Repositories;

namespace MoneyFox.Shared.ViewModels
{
    /// <summary>
    ///     This ViewModel is for the usage in the paymentlist when a concret account is selected
    /// </summary>
    public class PaymentListBalanceViewModel : BalanceViewModel
    {
        private readonly int accountId;
        private readonly IAccountRepository accountRepository;
        private readonly IEndOfMonthManager endOfMonthManager;

        public PaymentListBalanceViewModel(IAccountRepository accountRepository, IEndOfMonthManager endOfMonthManager,
            int accountId)
            : base(accountRepository, endOfMonthManager)
        {
            this.accountRepository = accountRepository;
            this.endOfMonthManager = endOfMonthManager;
            this.accountId = accountId;
        }

        /// <summary>
        ///     Calculates the sum of all accounts at the current moment.
        /// </summary>
        /// <returns>Sum of the balance of all accounts.</returns>
        protected override double GetTotalBalance()
            => accountRepository.FindById(accountId)?.CurrentBalance ?? 0;

        /// <summary>
        ///     Calculates the sum of the selected account at the end of the month.
        ///     This includes all payments coming until the end of month.
        /// </summary>
        /// <returns>Balance of the selected accont including all payments to come till end of month.</returns>
        protected override double GetEndOfMonthValue()
        {
            return endOfMonthManager.GetEndOfMonthBalanceForAccount(accountRepository.FindById(accountId));
        }
    }
}