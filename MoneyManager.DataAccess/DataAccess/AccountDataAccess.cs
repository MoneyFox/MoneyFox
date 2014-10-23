using System.Collections.ObjectModel;
using System.Linq;
using MoneyManager.DataAccess.Model;
using MoneyManager.Foundation;
using SQLite.Net;

namespace MoneyManager.DataAccess.DataAccess
{
    internal class AccountDataAccess : AbstractDataAccess<Account>
    {
        public Account SelectedAccount { get; set; }

        public ObservableCollection<Account> AllAccounts { get; set; }

        protected override void SaveToDb(Account itemToAdd)
        {
            using (SQLiteConnection dbConn = SqlConnectionFactory.GetSqlConnection())
            {
                if (AllAccounts == null)
                {
                    AllAccounts = new ObservableCollection<Account>();
                }

                AllAccounts.Add(itemToAdd);
                itemToAdd.Id = dbConn.Insert(itemToAdd);
            }
        }

        protected override void DeleteFromDatabase(Account itemToDelete)
        {
            using (SQLiteConnection dbConn = SqlConnectionFactory.GetSqlConnection())
            {
                AllAccounts.Remove(itemToDelete);
                dbConn.Delete(itemToDelete);
            }
        }

        protected override void GetListFromDb()
        {
            using (SQLiteConnection dbConn = SqlConnectionFactory.GetSqlConnection())
            {
                AllAccounts = new ObservableCollection<Account>(dbConn.Table<Account>().ToList());
            }
        }

        protected override void UpdateItem(Account itemToUpdate)
        {
            using (SQLiteConnection dbConn = SqlConnectionFactory.GetSqlConnection())
            {
                dbConn.Update(itemToUpdate, typeof (Account));
            }
        }
    }
}