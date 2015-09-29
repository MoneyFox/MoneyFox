using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using MoneyManager.Foundation;
using MoneyManager.Foundation.Interfaces;
using MoneyManager.Foundation.Model;
using PropertyChanged;
using SQLiteNetExtensions.Extensions;

namespace MoneyManager.DataAccess
{
    [ImplementPropertyChanged]
    public class AccountDataAccess : AbstractDataAccess<Account>
    {
        private readonly ISqliteConnectionCreator connectionCreator;

        public AccountDataAccess(ISqliteConnectionCreator connectionCreator)
        {
            this.connectionCreator = connectionCreator;
        }

        /// <summary>
        ///     Saves a Account to the database.
        /// </summary>
        /// <param name="itemToSave">Account to save.</param>
        protected override void SaveToDb(Account itemToSave)
        {
            using (var db = connectionCreator.GetConnection())
            {
                if (itemToSave.Id == 0)
                {
                    db.InsertWithChildren(itemToSave);
                }
                else
                {
                    db.UpdateWithChildren(itemToSave);
                }
            }
        }

        /// <summary>
        ///     Deletes an Account from the database.
        /// </summary>
        /// <param name="itemToDelete">Account to delete</param>
        protected override void DeleteFromDatabase(Account itemToDelete)
        {
            using (var db = connectionCreator.GetConnection())
            {
                db.Delete(itemToDelete);
            }
        }

        /// <summary>
        ///     Loads an list of accounts from the database filtered by the filter expression.
        /// </summary>
        /// <param name="filter">filter expression</param>
        /// <returns>List of loaded accounts.</returns>
        protected override List<Account> GetListFromDb(Expression<Func<Account, bool>> filter)
        {
            using (var db = connectionCreator.GetConnection())
            {
                return db.Table<Account>()
                    .OrderBy(x => x.Name)
                    .ToList();
            }
        }
    }
}