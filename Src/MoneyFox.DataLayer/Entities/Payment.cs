using System;
using System.ComponentModel.DataAnnotations;
using MoneyFox.Foundation;

namespace MoneyFox.DataLayer.Entities
{
    /// <summary>
    ///     Databasemodel for payments. Includes expenses, income and transfers.
    ///     Databasetable: Payments
    /// </summary>
    public class Payment
    {
        private Payment() { }

        public int Id { get; private set; }

        [Required]
        public int ChargedAccountId { get; private set; }

        public int? TargetAccountId { get; private set; }

        public int? CategoryId { get; private set; }

        public DateTime Date { get; private set; }
        public double Amount { get; private set; }
        public bool IsCleared { get; private set; }
        public PaymentType Type { get; private set; }
        public string Note { get; private set; }
        public bool IsRecurring { get; private set; }

        public int? RecurringPaymentId { get; private set; }

        public virtual Category Category { get; private set; }

        public virtual Account ChargedAccount { get; private set; }

        public virtual Account TargetAccount { get; private set; }
        
        public virtual RecurringPayment RecurringPayment { get; private set; }
    }
}