#region

using System;
using PropertyChanged;
using SQLite.Net.Attributes;

#endregion

namespace MoneyManager.Foundation.Model {
    [ImplementPropertyChanged]
    [Table("RecurringTransactiont")]
    public class RecurringTransaction {

        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public int ChargedAccountId { get; set; }

        public int TargetAccountId { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public bool IsEndless { get; set; }

        public double Amount { get; set; }

        public double AmountWithoutExchange { get; set; }

        public string Currency { get; set; }

        public int? CategoryId { get; set; }

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