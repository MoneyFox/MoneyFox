using System;
using SQLite;

namespace MoneyFox.Shared.Model
{
    [Table("RecurringPayments")]
    public class RecurringPayment
    {
        private Category category;

        private Account chargedAccount;

        private Account targetAccount;

        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public int ChargedAccountId { get; set; }

        public int TargetAccountId { get; set; }

        public int? CategoryId { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool IsEndless { get; set; }
        public double Amount { get; set; }
        public int Type { get; set; }
        public int Recurrence { get; set; }
        public string Note { get; set; }

        /// <summary>
        ///     In case it's a expense or transfer the account who will be charged.
        ///     In case it's an income the account who will be credited.
        /// </summary>
        [Ignore]
        public Account ChargedAccount
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
        ///     The <see cref="Account" /> who will be credited by a transfer.
        ///     Not used for the other payment types.
        /// </summary>
        [Ignore]
        public Account TargetAccount
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
        [Ignore]
        public Category Category
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