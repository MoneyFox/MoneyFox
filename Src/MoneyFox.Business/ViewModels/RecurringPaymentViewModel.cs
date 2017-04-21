using System;
using MoneyFox.Business.ViewModels;

namespace MoneyFox.Foundation.DataModels
{
    public class RecurringPaymentViewModel
    {
        private CategoryViewModel category;

        private AccountViewModel chargedAccount;

        private AccountViewModel targetAccount;

        public int Id { get; set; }

        public int ChargedAccountId { get; set; }

        public int TargetAccountId { get; set; }

        public int? CategoryId { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool IsEndless { get; set; }
        public double Amount { get; set; }
        public PaymentType Type { get; set; }
        public PaymentRecurrence Recurrence { get; set; }
        public string Note { get; set; }

        /// <summary>
        ///     In case it's a expense or transfer the account who will be charged.
        ///     In case it's an income the account who will be credited.
        /// </summary>
        public AccountViewModel ChargedAccount
        {
            get { return chargedAccount; }
            set
            {
                if (chargedAccount != value)
                {
                    chargedAccount = value;
                    ChargedAccountId = value?.Id ?? 0;
                }
            }
        }


        /// <summary>
        ///     The <see cref="AccountViewModel" /> who will be credited by a transfer.
        ///     Not used for the other payment types.
        /// </summary>
        public AccountViewModel TargetAccount
        {
            get { return targetAccount; }
            set
            {
                if (targetAccount != value)
                {
                    targetAccount = value;
                    TargetAccountId = value?.Id ?? 0;
                }
            }
        }

        /// <summary>
        ///     The <see cref="Category" /> for this payment
        /// </summary>
        public CategoryViewModel Category
        {
            get { return category; }
            set
            {
                if (category != value)
                {
                    category = value;
                    CategoryId = value?.Id ?? 0;
                }
            }
        }
    }
}