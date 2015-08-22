using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using MoneyManager.Foundation;
using MoneyManager.Foundation.Model;
using MoneyManager.Foundation.OperationContracts;
using PropertyChanged;
using SQLiteNetExtensions.Extensions;

namespace MoneyManager.Business.DataAccess
{
    [ImplementPropertyChanged]
    public class AccountDataAccess : AbstractDataAccess<Account>
    {
        private readonly IDbHelper dbHelper;

        public AccountDataAccess(IDbHelper dbHelper)
        {
            this.dbHelper = dbHelper;
        }

        /// <summary>
        ///     Saves a Account to the database.
        /// </summary>
        /// <param name="itemToSave">Account to save.</param>
        protected override void SaveToDb(Account itemToSave)
        {
            using (var db = dbHelper.GetSqlConnection())
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
            using (var db = dbHelper.GetSqlConnection())
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
            using (var db = dbHelper.GetSqlConnection())
            {
                return db.Table<Account>()
                    .OrderBy(x => x.Name)
                    .ToList();
            }
        }
    }
}