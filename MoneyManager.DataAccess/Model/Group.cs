using Microsoft.Practices.ServiceLocation;
using MoneyManager.DataAccess.DataAccess;
using SQLite.Net.Attributes;
using System.Collections.Generic;
using System.Linq;

namespace MoneyManager.DataAccess.Model
{
    [Table("Groups")]
    internal class Group
    {
        private AccountDataAccess AccountDataAccess
        {
            get { return ServiceLocator.Current.GetInstance<AccountDataAccess>(); }
        }

        [PrimaryKey, AutoIncrement, Indexed]
        public int Id { get; set; }

        public string Name { get; set; }

        public List<Account> Accounts
        {
            get { return AccountDataAccess.AllAccounts.Where(x => x.GroupId == Id).ToList(); }
        }
    }
}