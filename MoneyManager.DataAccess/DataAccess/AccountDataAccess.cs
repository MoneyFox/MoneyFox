#region

using System.Collections.ObjectModel;
using System.Linq;
using MoneyManager.Foundation;
using MoneyManager.Foundation.Model;
using PropertyChanged;
using SQLite.Net;
using SQLiteNetExtensions.Extensions;

#endregion

namespace MoneyManager.DataAccess.DataAccess {
    [ImplementPropertyChanged]
    public class AccountDataAccess : AbstractDataAccess<Account> {
        public Account SelectedAccount { get; set; }

        public ObservableCollection<Account> AllAccounts { get; set; }

        protected override void SaveToDb(Account itemToSave) {
            using (var db = SqlConnectionFactory.GetSqlConnection()) {
                if (itemToSave.Id == 0) {
                    db.InsertWithChildren(itemToSave);
                } else {
                    db.UpdateWithChildren(itemToSave);
                }
            }
        }

        protected override void DeleteFromDatabase(Account itemToDelete) {
            using (var db = SqlConnectionFactory.GetSqlConnection()) {
                AllAccounts.Remove(itemToDelete);
                db.Delete(itemToDelete);
            }
        }

        protected override void GetListFromDb() {
            using (var db = SqlConnectionFactory.GetSqlConnection()) {
                AllAccounts = new ObservableCollection<Account>(db.Table<Account>()
                    .ToList()
                    .OrderBy(x => x.Name));
            }
        }
    }
}