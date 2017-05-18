using System;
using System.ComponentModel.DataAnnotations;

namespace MoneyFox.DataAccess.EntityOld
{
    internal class Payment
    {
        [Key]
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

        public Account ChargedAccount { get; set; }

        public Account TargetAccount { get; set; }

        public Category Category { get; set; }

        public RecurringPayment RecurringPayment { get; set; }
    }
}
