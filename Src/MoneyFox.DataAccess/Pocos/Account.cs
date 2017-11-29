using System.Collections.Generic;
using MoneyFox.DataAccess.Entities;

namespace MoneyFox.DataAccess.Pocos
{
    /// <summary>
    ///     Business object for account data.
    /// </summary>
    public class Account
    {
        /// <summary>
        ///     Default constructor. Will Create a new AccountEntity
        /// </summary>
        public Account()
        {
            Data = new AccountEntity
            {
                ChargedPayments = new List<PaymentEntity>(),
                TargetedPayments = new List<PaymentEntity>(),
                ChargedRecurringPayments = new List<RecurringPaymentEntity>(),
                TargetedRecurringPayments = new List<RecurringPaymentEntity>()
            };
        }

        /// <summary>
        ///     Set the data for this object.
        /// </summary>
        /// <param name="account">Account data to wrap.</param>
        public Account(AccountEntity account)
        {
            Data = account;
        }

        /// <summary>
        ///     Accountdata
        /// </summary>
        public AccountEntity Data { get; set; }
    }
}
