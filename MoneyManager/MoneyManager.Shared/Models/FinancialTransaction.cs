using MoneyManager.Models;
using PropertyChanged;
using SQLite;
using System;
using System.Linq;

namespace MoneyTracker.Models
{
    [ImplementPropertyChanged]
    [Table("FinancialTransactions")]
    public class FinancialTransaction
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public int ChargedAccountId { get; set; }

        public int TargetAccountId { get; set; }

        public DateTime Date { get; set; }

        public double Amount { get; set; }

        public string Currency { get; set; }

        public bool Cleared { get; set; }

        public int Type { get; set; }

        public string Note { get; set; }

        public bool IsRecurrence { get; set; }

        public int ReccuringTransactionId { get; set; }

        [Ignore]
        public Account ChargedAccount
        {
            get { return App.AccountViewModel.AllAccounts.FirstOrDefault(x => x.Id == Id); }
            set { ChargedAccountId = value.Id; }
        }

        [Ignore]
        public Account TargetAccount
        {
            get { return App.AccountViewModel.AllAccounts.FirstOrDefault(x => x.Id == Id); }
            set { ChargedAccountId = value.Id; }
        }

        [Ignore]
        public RecurringTransaction RecurringTransaction
        {
            get { return App.RecurrenceTransactionViewModel.AllTransactions.FirstOrDefault(x => x.Id == Id); }
            set { ReccuringTransactionId = value.Id; }
        }

        [Ignore]
        public bool ClearTransactionNow
        {
            get
            {
                return Date.Date <= DateTime.Now.Date;
            }
        }
    }
}