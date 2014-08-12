using Microsoft.Practices.ServiceLocation;
using MoneyManager.DataAccess;
using PropertyChanged;
using SQLite;
using System.Collections.Generic;
using System.Linq;

namespace MoneyManager.Models
{
    [Table("Groups")]
    [ImplementPropertyChanged]
    public class Group
    {
        private AccountDataAccess AccountDataAccess
        {
            get
            {
                return ServiceLocator.Current.GetInstance<AccountDataAccess>();
            }
        }

        [PrimaryKey, AutoIncrement, Indexed]
        public int Id { get; set; }

        public string Name { get; set; }

        public List<Account> Accounts
        {
            get
            {
                return AccountDataAccess.AllAccounts.Where(x => x.GroupId == Id).ToList();
            }
        }
    }
}