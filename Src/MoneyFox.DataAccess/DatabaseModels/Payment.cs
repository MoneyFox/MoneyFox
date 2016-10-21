using System;
using SQLite;
using SQLiteNetExtensions.Attributes;

namespace MoneyFox.DataAccess.DatabaseModels
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

        public int ChargedAccountId { get; set; }
        public int TargetAccountId { get; set; }

        [ForeignKey(typeof(Category))]
        public int? CategoryId { get; set; }
        public DateTime Date { get; set; }
        public double Amount { get; set; }
        public bool IsCleared { get; set; }
        public int Type { get; set; }
        public string Note { get; set; }
        public bool IsRecurring { get; set; }
        public int RecurringPaymentId { get; set; }
        public int CurrentAccountId { get; set; }

        [ManyToOne(CascadeOperations = CascadeOperation.All)]
        public Category Category { get; set; }
    }
}