using System;
using PropertyChanged;
using SQLite.Net.Attributes;
using SQLiteNetExtensions.Attributes;

namespace MoneyManager.Foundation.Model
{
    [ImplementPropertyChanged]
    [Table("RecurringTransaction")]
    public class RecurringTransaction
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        [ForeignKey(typeof (Account))]
        public int ChargedAccountId { get; set; }

        [ForeignKey(typeof (Account))]
        public int TargetAccountId { get; set; }

        [ForeignKey(typeof (Category))]
        public int? CategoryId { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool IsEndless { get; set; }
        public double Amount { get; set; }
        public int Type { get; set; }
        public int Recurrence { get; set; }
        public string Note { get; set; }

        [ManyToOne]
        public Account ChargedAccount { get; set; }

        [ManyToOne]
        public Account TargetAccount { get; set; }

        [ManyToOne]
        public Category Category { get; set; }
    }
}