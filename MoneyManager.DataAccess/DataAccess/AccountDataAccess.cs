#region

using System.Collections.ObjectModel;
using System.Linq;
using MoneyManager.Foundation;
using MoneyManager.Foundation.Model;
using PropertyChanged;
using SQLite.Net;

#endregion

namespace MoneyManager.DataAccess.DataAccess {
    [ImplementPropertyChanged]
    public class AccountDataAccess : AbstractDataAccess<Account> {
        public Account SelectedAccount { get; set; }

        public ObservableCollection<Account> AllAccounts { get; set; }

        protected override void SaveToDb(Account itemToAdd) {
            using (SQLiteConnection dbConn = SqlConnectionFactory.GetSqlConnection()) {
                if (AllAccounts == null) {
                    AllAccounts = new ObservableCollection<Account>();
                }

                AllAccounts.Add(itemToAdd);
                AllAccounts = new ObservableCollection<Account>(AllAccounts.OrderBy(x => x.Name));
                dbConn.Insert(itemToAdd);
            }
        }

        protected override void DeleteFromDatabase(Account itemToDelete) {
            using (SQLiteConnection dbConn = SqlConnectionFactory.GetSqlConnection()) {
                AllAccounts.Remove(itemToDelete);
                dbConn.Delete(itemToDelete);
            }
        }

        protected override void GetListFromDb() {
            using (SQLiteConnection dbConn = SqlConnectionFactory.GetSqlConnection()) {
                AllAccounts = new ObservableCollection<Account>(dbConn.Table<Account>()
                    .ToList()
                    .OrderBy(x => x.Name));
            }
        }
    }
}