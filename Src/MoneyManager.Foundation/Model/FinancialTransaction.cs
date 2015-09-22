using System;
using PropertyChanged;
using SQLite.Net.Attributes;
using SQLiteNetExtensions.Attributes;

namespace MoneyManager.Foundation.Model
{
    [ImplementPropertyChanged]
    [Table("FinancialTransactions")]
    public class FinancialTransaction
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        [ForeignKey(typeof (Account))]
        public int ChargedAccountId { get; set; }

        [ForeignKey(typeof (Account))]
        public int TargetAccountId { get; set; }

        [ForeignKey(typeof (Category))]
        public int? CategoryId { get; set; }

        public DateTime Date { get; set; }
        public double Amount { get; set; }
        public bool IsCleared { get; set; }
        public int Type { get; set; }
        public string Note { get; set; }
        public bool IsRecurring { get; set; }

        [ForeignKey(typeof (RecurringTransaction))]
        public int ReccuringTransactionId { get; set; }

        [ManyToOne("ChargedAccountId", CascadeOperations = CascadeOperation.All)]
        public Account ChargedAccount { get; set; }

        [ManyToOne("TargetAccountId", CascadeOperations = CascadeOperation.All)]
        public Account TargetAccount { get; set; }

        [ManyToOne]
        public Category Category { get; set; }

        [ManyToOne("ReccuringTransactionId", CascadeOperations = CascadeOperation.All)]
        public RecurringTransaction RecurringTransaction { get; set; }

        [Ignore]
        public bool ClearTransactionNow => Date.Date <= DateTime.Now.Date;

        [Ignore]
        public bool IsTransfer => Type == (int) TransactionType.Transfer;
    }
}