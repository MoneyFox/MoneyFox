using System;
using SQLite.Net.Attributes;

namespace MoneyFox.DataAccess.DatabaseModels
{
    [Table("RecurringPayments")]
    internal class RecurringPayment
    {
        [PrimaryKey, AutoIncrement, Indexed]
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
    }
}