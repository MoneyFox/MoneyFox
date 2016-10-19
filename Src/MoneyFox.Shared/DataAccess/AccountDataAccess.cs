using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using MoneyFox.Shared.Interfaces;
using MoneyFox.Shared.Model;

namespace MoneyFox.Shared.DataAccess
{
    public class AccountDataAccess : AbstractDataAccess<AccountViewModel>
    {
        private readonly IDatabaseManager dbManager;

        public AccountDataAccess(IDatabaseManager dbManager)
        {
            this.dbManager = dbManager;
        }

        /// <summary>
        ///     Saves a AccountViewModel to the database.
        /// </summary>
        /// <param name="itemToSave">AccountViewModel to save.</param>
        protected override void SaveToDb(AccountViewModel itemToSave)
        {
            using (var dbConnection = dbManager.GetConnection())
            {
                //Don't use InsertOrReplace here, because it will always replace the first element
                if (itemToSave.Id == 0)
                {
                    dbConnection.Insert(itemToSave);
                    itemToSave.Id = dbConnection.Table<AccountViewModel>().OrderByDescending(x => x.Id).First().Id;
                }
                else
                {
                    dbConnection.Update(itemToSave);
                }
            }
        }

        /// <summary>
        ///     Deletes an AccountViewModel from the database.
        /// </summary>
        /// <param name="payment">AccountViewModel to delete</param>
        protected override void DeleteFromDatabase(AccountViewModel payment)
        {
            using (var dbConnection = dbManager.GetConnection())
            {
                dbConnection.Delete(payment);
            }
        }

        /// <summary>
        ///     Loads an list of accounts from the database filtered by the filter expression.
        /// </summary>
        /// <param name="filter">filter expression</param>
        /// <returns>List of loaded accounts.</returns>
        protected override List<AccountViewModel> GetListFromDb(Expression<Func<AccountViewModel, bool>> filter)
        {
            using (var dbConnection = dbManager.GetConnection())
            {
                var listQuery = dbConnection.Table<AccountViewModel>();

                if (filter != null)
                {
                    listQuery = listQuery.Where(filter);
                }

                return listQuery.OrderBy(x => x.Name).ToList();
            }
        }
    }
}