using Microsoft.Practices.ServiceLocation;
using MoneyManager.DataAccess;
using MoneyManager.DataAccess.DataAccess;
using MoneyManager.DataAccess.Model;
using MoneyManager.Models;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using SQLite.Net.Attributes;

namespace MoneyManager.DataAccess.Model
{
    [Table("RecurringTransactiont")]
    internal class RecurringTransaction
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

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public bool IsEndless { get; set; }

        public double Amount { get; set; }

        public string Currency { get; set; }

        public int? CategoryId { get; set; }

        public int Type { get; set; }

        public int Recurrence { get; set; }

        public string Note { get; set; }

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
    }
}