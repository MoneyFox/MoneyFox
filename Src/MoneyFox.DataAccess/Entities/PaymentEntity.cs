using System;
using MoneyFox.Foundation;

namespace MoneyFox.DataAccess.Entities
{
    /// <summary>
    ///     Databasemodel for payments. Includes expenses, income and transfers.
    ///     Databasetable: Payments
    /// </summary>
    public class PaymentEntity
    {
        public int Id { get; set; }

        public int ChargedAccountId { get; set; }

        public int? TargetAccountId { get; set; }

        public int? CategoryId { get; set; }

        public DateTime Date { get; set; }
        public double Amount { get; set; }
        public bool IsCleared { get; set; }
        public PaymentType Type { get; set; }
        public string Note { get; set; }
        public bool IsRecurring { get; set; }

        public int? RecurringPaymentId { get; set; }

        public virtual CategoryEntity Category { get; set; }

        public virtual AccountEntity ChargedAccount { get; set; }

        public virtual AccountEntity TargetAccount { get; set; }

        public virtual RecurringPaymentEntity RecurringPayment { get; set; }
    }
}