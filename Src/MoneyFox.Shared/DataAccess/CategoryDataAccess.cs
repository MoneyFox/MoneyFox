using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using MoneyFox.Shared.Interfaces;
using MoneyFox.Shared.Model;

namespace MoneyFox.Shared.DataAccess
{
    public class CategoryDataAccess : AbstractDataAccess<CategoryViewModel>
    {
        private readonly IDatabaseManager dbManager;

        public CategoryDataAccess(IDatabaseManager dbManager)
        {
            this.dbManager = dbManager;
        }

        /// <summary>
        ///     Saves an CategoryViewModel to database
        /// </summary>
        /// <param name="itemToSave">CategoryViewModel to save.</param>
        protected override void SaveToDb(CategoryViewModel itemToSave)
        {
            using (var dbConnection = dbManager.GetConnection())
            {
                //Don't use insert or replace here, because it will always replace the first element
                if (itemToSave.Id == 0)
                {
                    dbConnection.Insert(itemToSave);
                    itemToSave.Id = dbConnection.Table<CategoryViewModel>().OrderByDescending(x => x.Id).First().Id;
                }
                else
                {
                    dbConnection.Update(itemToSave);
                }
            }
        }

        /// <summary>
        ///     DeleteItem an item from the database
        /// </summary>
        /// <param name="category">CategoryViewModel to delete.</param>
        protected override void DeleteFromDatabase(CategoryViewModel category)
        {
            using (var dbConnection = dbManager.GetConnection())
            {
                dbConnection.Delete(category);
            }
        }

        /// <summary>
        ///     Loads a list of Categories from the database
        /// </summary>
        /// <param name="filter">>Filter expression</param>
        /// <returns>Loaded categories.</returns>
        protected override List<CategoryViewModel> GetListFromDb(Expression<Func<CategoryViewModel, bool>> filter)
        {
            using (var dbConnection = dbManager.GetConnection())
            {
                var listQuery = dbConnection.Table<CategoryViewModel>();

                if (filter != null)
                {
                    listQuery = listQuery.Where(filter);
                }

                return listQuery.OrderBy(x => x.Name).ToList();
            }
        }
    }
}