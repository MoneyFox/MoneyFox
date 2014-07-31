using PropertyChanged;
using SQLite;
using System;

namespace MoneyManager.Models
{
    [ImplementPropertyChanged]
    [Table("RecurringTransactions")]
    public class RecurringTransaction
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public int TransactionId { get; set; }

        public string RecurringType { get; set; }
    }
}