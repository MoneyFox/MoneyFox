using System;
using SQLite.Net.Attributes;

namespace MoneyManager.Foundation.Model
{
    [Table("RecurringPayments")]
    public class RecurringPayment
    {
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

        [Ignore]
        public Account ChargedAccount { get; set; }

        [Ignore]
        public Account TargetAccount { get; set; }

        [Ignore]
        public Category Category { get; set; }
    }
}