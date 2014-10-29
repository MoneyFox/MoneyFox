using MoneyManager.DataAccess.Model;
using MoneyManager.Foundation;
using PropertyChanged;
using System.Collections.ObjectModel;
using System.Linq;

namespace MoneyManager.DataAccess.DataAccess
{
    [ImplementPropertyChanged]
    public class AccountDataAccess : AbstractDataAccess<Account>
    {
        public Account SelectedAccount { get; set; }

        public ObservableCollection<Account> AllAccounts { get; set; }

        protected override void SaveToDb(Account itemToAdd)
        {
            using (var dbConn = SqlConnectionFactory.GetSqlConnection())
            {
                if (AllAccounts == null)
                {
                    AllAccounts = new ObservableCollection<Account>();
                }

                AllAccounts.Add(itemToAdd);
                AllAccounts = new ObservableCollection<Account>(AllAccounts.OrderBy(x => x.Name));
                itemToAdd.Id = dbConn.Insert(itemToAdd);
            }
        }

        protected override void DeleteFromDatabase(Account itemToDelete)
        {
            using (var dbConn = SqlConnectionFactory.GetSqlConnection())
            {
                AllAccounts.Remove(itemToDelete);
                dbConn.Delete(itemToDelete);
            }
        }

        protected override void GetListFromDb()
        {
            using (var dbConn = SqlConnectionFactory.GetSqlConnection())
            {
                AllAccounts = new ObservableCollection<Account>(dbConn.Table<Account>()
                    .ToList()
                    .OrderBy(x => x.Name));
            }
        }

        protected override void UpdateItem(Account itemToUpdate)
        {
            using (var dbConn = SqlConnectionFactory.GetSqlConnection())
            {
                dbConn.Update(itemToUpdate, typeof(Account));
            }
        }
    }
}