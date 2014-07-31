using MoneyManager.ViewModels;
using MoneyManager.ViewModels.Data;
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
        private AccountViewModel accountViewModel
        {
            get { return new ViewModelLocator().AccountViewModel; }
        }

        [PrimaryKey, AutoIncrement, Indexed]
        public int Id { get; set; }

        public string Name { get; set; }

        public List<Account> Accounts
        {
            get
            {
                return accountViewModel.AllAccounts.Where(x => x.GroupId == Id).ToList();
            }
        }
    }
}