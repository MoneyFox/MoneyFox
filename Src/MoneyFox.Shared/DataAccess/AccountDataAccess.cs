using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using MoneyFox.Shared.Model;
using PropertyChanged;
using SQLite.Net;

namespace MoneyFox.Shared.DataAccess
{
    [ImplementPropertyChanged]
    public class AccountDataAccess : AbstractDataAccess<Account>
    {
        private readonly SQLiteConnection dbConnection;

        public AccountDataAccess(SQLiteConnection dbConnection)
        {
            this.dbConnection = dbConnection;
        }

        /// <summary>
        ///     Saves a Account to the database.
        /// </summary>
        /// <param name="itemToSave">Account to save.</param>
        protected override void SaveToDb(Account itemToSave)
        {
            //Don't use InsertOrReplace here, because it will always replace the first element
            if (itemToSave.Id == 0)
            {
                dbConnection.Insert(itemToSave);
                itemToSave.Id = dbConnection.Table<Account>().OrderByDescending(x => x.Id).First().Id;
            }
            else
            {
                dbConnection.Update(itemToSave);
            }
        }

        /// <summary>
        ///     Deletes an Account from the database.
        /// </summary>
        /// <param name="payment">Account to delete</param>
        protected override void DeleteFromDatabase(Account payment)
        {
            dbConnection.Delete(payment);
        }

        /// <summary>
        ///     Loads an list of accounts from the database filtered by the filter expression.
        /// </summary>
        /// <param name="filter">filter expression</param>
        /// <returns>List of loaded accounts.</returns>
        protected override List<Account> GetListFromDb(Expression<Func<Account, bool>> filter)
        {
            var listQuery = dbConnection.Table<Account>();

            if (filter != null)
            {
                listQuery = listQuery.Where(filter);
            }

            return listQuery.OrderBy(x => x.Name).ToList();
        }
    }
}