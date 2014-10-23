using MoneyManager.DataAccess.Model;
using MoneyManager.Foundation;
using SQLite.Net;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace MoneyManager.DataAccess.DataAccess
{
    internal class AccountDataAccess : AbstractDataAccess<Account>
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
                AllAccounts = new ObservableCollection<Account>(dbConn.Table<Account>().ToList());
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