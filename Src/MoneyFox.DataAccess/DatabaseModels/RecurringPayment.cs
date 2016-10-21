using System;
using SQLite;
using SQLiteNetExtensions.Attributes;

namespace MoneyFox.DataAccess.DatabaseModels
{
    [Table("RecurringPayments")]
    internal class RecurringPayment
    {
        [PrimaryKey, AutoIncrement, Indexed]
        public int Id { get; set; }

        [ForeignKey(typeof(Account), Name = nameof(ChargedAccount))]
        public int ChargedAccountId { get; set; }

        [ForeignKey(typeof(Account), Name = nameof(TargetAccount))]
        public int TargetAccountId { get; set; }

        [ForeignKey(typeof(Category))]
        public int? CategoryId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool IsEndless { get; set; }
        public double Amount { get; set; }
        public int Type { get; set; }
        public int Recurrence { get; set; }
        public string Note { get; set; }

        [ManyToOne(CascadeOperations = CascadeOperation.CascadeRead)]
        public Category Category { get; set; }

        [ManyToOne(CascadeOperations = CascadeOperation.CascadeRead)]
        public Account ChargedAccount { get; set; }

        [ManyToOne(CascadeOperations = CascadeOperation.CascadeRead)]
        public Account TargetAccount { get; set; }
    }
}