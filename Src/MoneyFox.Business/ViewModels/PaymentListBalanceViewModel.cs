using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Azure.Mobile.Analytics;
using MoneyFox.Business.Manager;
using MoneyFox.DataAccess.DataServices;

namespace MoneyFox.Business.ViewModels
{
    /// <summary>
    ///     This ViewModel is for the usage in the paymentlist when a concret account is selected
    /// </summary>
    public class PaymentListBalanceViewModel : BalanceViewModel
    {
        private readonly int accountId;
        private readonly IAccountService accountService;
        private readonly IBalanceCalculationManager balanceCalculationManager;

        /// <summary>
        ///     Constructor
        /// </summary>
        public PaymentListBalanceViewModel(IAccountService accountService, IBalanceCalculationManager balanceCalculationManager,
            int accountId)
            : base(balanceCalculationManager)
        {
            this.accountService = accountService;
            this.balanceCalculationManager = balanceCalculationManager;
            this.accountId = accountId;
        }

        /// <summary>
        ///     Calculates the sum of all accounts at the current moment.
        /// </summary>
        /// <returns>Sum of the balance of all accounts.</returns>
        protected override async Task<double> GetTotalBalance()
        {
            try
            {
                var account = await accountService.GetById(accountId);
                return account.Data.CurrentBalance;
            }
            catch (Exception ex)
            {
                Analytics.TrackEvent("Exception: On Token refresh", new Dictionary<string, string>
                {
                    {"Message", ex.Message},
                    {"Stacktrace", ex.StackTrace}
                });
            }
            return 0;
        }

        /// <summary>
        ///     Calculates the sum of the selected account at the end of the month.
        ///     This includes all payments coming until the end of month.
        /// </summary>
        /// <returns>Balance of the selected accont including all payments to come till end of month.</returns>
        protected override async Task<double> GetEndOfMonthValue()
        {
            return await balanceCalculationManager.GetEndOfMonthBalanceForAccount(await accountService.GetById(accountId));
        }
    }
}