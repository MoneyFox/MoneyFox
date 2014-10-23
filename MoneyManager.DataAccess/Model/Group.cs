using System.Collections.Generic;
using System.Linq;
using Microsoft.Practices.ServiceLocation;
using MoneyManager.DataAccess.DataAccess;
using PropertyChanged;
using SQLite.Net.Attributes;

namespace MoneyManager.DataAccess.Model
{
    [Table("Groups")]
    [ImplementPropertyChanged]
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