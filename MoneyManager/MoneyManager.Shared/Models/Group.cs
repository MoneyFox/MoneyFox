using PropertyChanged;
using SQLite;
using System.Collections.Generic;
using System.Linq;

namespace MoneyTracker.Models
{
    [Table("Groups")]
    [ImplementPropertyChanged]
    public class Group
    {
        [PrimaryKey, AutoIncrement, Indexed]
        public int Id { get; set; }

        public string Name { get; set; }

        public List<Account> Accounts
        {
            get
            {
                return App.AccountViewModel.AllAccounts.Where(x => x.GroupId == Id).ToList();
            }
        }
    }
}