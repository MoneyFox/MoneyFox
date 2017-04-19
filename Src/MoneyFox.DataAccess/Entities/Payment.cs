using System;

namespace MoneyFox.DataAccess.Entities
{
    /// <summary>
    ///     Databasemodel for payments. Includes expenses, income and transfers.
    ///     Databasetable: Payments
    /// </summary>
    internal class Payment
    {
        public int Id { get; set; }

        public int ChargedAccountId { get; set; }

        public int TargetAccountId { get; set; }

        public int? CategoryId { get; set; }

        public DateTime Date { get; set; }
        public double Amount { get; set; }
        public bool IsCleared { get; set; }
        public int Type { get; set; }
        public string Note { get; set; }
        public bool IsRecurring { get; set; }

        public int RecurringPaymentId { get; set; }

        public virtual Category Category { get; set; }

        public virtual Account ChargedAccount { get; set; }

        public virtual Account TargetAccount { get; set; }

        public virtual RecurringPayment RecurringPayment { get; set; }
    }
}