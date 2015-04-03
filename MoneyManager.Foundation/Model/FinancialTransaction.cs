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

        [ForeignKey(typeof(Account))]
        public int ChargedAccountId { get; set; }

        [ForeignKey(typeof(Account))]
        public int TargetAccountId { get; set; }

        [ForeignKey(typeof (Category))]
        public int? CategoryId { get; set; }

        public DateTime Date { get; set; }
        public double AmountWithoutExchange { get; set; }
        public double Amount { get; set; }
        public bool IsExchangeModeActive { get; set; }
        public double ExchangeRatio { get; set; }
        public string Currency { get; set; }
        public bool Cleared { get; set; }
        public int Type { get; set; }
        public string Note { get; set; }
        public bool IsRecurring { get; set; }

        [ForeignKey(typeof (RecurringTransaction))]
        public int? ReccuringTransactionId { get; set; }

        private Account _chargedAccount;
        [ManyToOne]
        public Account ChargedAccount {
            get { return _chargedAccount; }
            set {
                _chargedAccount = value;
                ChargedAccountId = value.Id;
            } }

        [ManyToOne]
        public Account TargetAccount { get; set; }

        [ManyToOne]
        public Category Category { get; set; }

        [ManyToOne]
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