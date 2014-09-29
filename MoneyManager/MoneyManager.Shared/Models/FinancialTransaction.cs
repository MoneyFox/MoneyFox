using Microsoft.Practices.ServiceLocation;
using MoneyManager.DataAccess;
using PropertyChanged;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MoneyManager.Models
{
    [ImplementPropertyChanged]
    [Table("FinancialTransactions")]
    public class FinancialTransaction
    {
        private IEnumerable<Account> allAccounts
        {
            get { return ServiceLocator.Current.GetInstance<AccountDataAccess>().AllAccounts; }
        }

        private IEnumerable<Category> allCategories
        {
            get { return ServiceLocator.Current.GetInstance<CategoryDataAccess>().AllCategories; }
        }

        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public int ChargedAccountId { get; set; }

        public DateTime Date { get; set; }

        public double Amount { get; set; }

        public string Currency { get; set; }

        public int? CategoryId { get; set; }

        public bool Cleared { get; set; }

        public int Type { get; set; }

        public string Note { get; set; }

        public bool IsRecurring { get; set; }

        public int ReccuringTransactionId { get; set; }

        [Ignore]
        public Account ChargedAccount
        {
            get { return allAccounts.FirstOrDefault(x => x.Id == ChargedAccountId); }
            set { ChargedAccountId = value.Id; }
        }

        [Ignore]
        public Category Category
        {
            get { return allCategories.FirstOrDefault(x => x.Id == CategoryId); }
            set
            {
                CategoryId = value == null
                    ? (int?)null
                    : value.Id;
            }
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