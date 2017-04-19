using System;
using SQLite;
using SQLiteNetExtensions.Attributes;

namespace MoneyFox.DataAccess.Entities
{
    /// <summary>
    ///     Databasemodel for payments. Includes expenses, income and transfers.
    ///     Databasetable: Payments
    /// </summary>
    [Table("Payments")]
    internal class Payment
    {
        [PrimaryKey, AutoIncrement, Indexed]
        public int Id { get; set; }

        [ForeignKey(typeof(Account), Name = nameof(ChargedAccount))]
        public int ChargedAccountId { get; set; }

        [ForeignKey(typeof(Account), Name = nameof(TargetAccount))]
        public int TargetAccountId { get; set; }

        [ForeignKey(typeof(Category))]
        public int? CategoryId { get; set; }

        public DateTime Date { get; set; }
        public double Amount { get; set; }
        public bool IsCleared { get; set; }
        public int Type { get; set; }
        public string Note { get; set; }
        public bool IsRecurring { get; set; }

        [ForeignKey(typeof(RecurringPayment))]
        public int RecurringPaymentId { get; set; }

        [ManyToOne(CascadeOperations = CascadeOperation.CascadeRead)]
        public Category Category { get; set; }

        [ManyToOne(nameof(ChargedAccountId), CascadeOperations = CascadeOperation.CascadeRead)]
        public Account ChargedAccount { get; set; }

        [ManyToOne(nameof(TargetAccountId), CascadeOperations = CascadeOperation.CascadeRead)]
        public Account TargetAccount { get; set; }

        [OneToOne(CascadeOperations = CascadeOperation.All)]
        public RecurringPayment RecurringPayment { get; set; }
    }
}