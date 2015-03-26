#region

using System;
using PropertyChanged;
using SQLite.Net.Attributes;
using SQLiteNetExtensions.Attributes;

#endregion

namespace MoneyManager.Foundation.Model {
    [ImplementPropertyChanged]
    [Table("FinancialTransactions")]
    public class FinancialTransaction {

        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public int ChargedAccountId { get; set; }

        public int TargetAccountId { get; set; }

        public DateTime Date { get; set; }

        public double AmountWithoutExchange { get; set; }

        public double Amount { get; set; }

        public bool IsExchangeModeActive { get; set; }

        public double ExchangeRatio { get; set; }

        public string Currency { get; set; }

        [ForeignKey(typeof(Category))]
        public int? CategoryId { get; set; }

        public bool Cleared { get; set; }

        public int Type { get; set; }

        public string Note { get; set; }

        public bool IsRecurring { get; set; }

        public int? ReccuringTransactionId { get; set; }

        [Ignore]
        public Account ChargedAccount { get; set; }

        [Ignore]
        public Account TargetAccount { get; set; }

        [ManyToOne]
        public Category Category { get; set; }

        [Ignore]
        public RecurringTransaction RecurringTransaction { get; set; }

        [Ignore]
        public bool ClearTransactionNow {
            get { return Date.Date <= DateTime.Now.Date; }
        }

        [Ignore]
        public bool IsTransfer {
            get { return Type == (int) TransactionType.Transfer; }
        }
    }
}