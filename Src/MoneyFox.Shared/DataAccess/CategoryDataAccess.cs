using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using MoneyFox.Shared.Interfaces;
using MoneyFox.Shared.Model;
using PropertyChanged;

namespace MoneyFox.Shared.DataAccess
{
    [ImplementPropertyChanged]
    public class CategoryDataAccess : AbstractDataAccess<Category>
    {
        private readonly IDatabaseManager dbManager;

        public CategoryDataAccess(IDatabaseManager dbManager)
        {
            this.dbManager = dbManager;
        }

        /// <summary>
        ///     Saves an Category to database
        /// </summary>
        /// <param name="itemToSave">Category to save.</param>
        protected override void SaveToDb(Category itemToSave)
        {
            using (var dbConnection = dbManager.GetConnection())
            {
                //Don't use insert or replace here, because it will always replace the first element
                if (itemToSave.Id == 0)
                {
                    dbConnection.Insert(itemToSave);
                    itemToSave.Id = dbConnection.Table<Category>().OrderByDescending(x => x.Id).First().Id;
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
        /// <param name="category">Category to delete.</param>
        protected override void DeleteFromDatabase(Category category)
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
        protected override List<Category> GetListFromDb(Expression<Func<Category, bool>> filter)
        {
            using (var dbConnection = dbManager.GetConnection())
            {
                var listQuery = dbConnection.Table<Category>();

                if (filter != null)
                {
                    listQuery = listQuery.Where(filter);
                }

                return listQuery.OrderBy(x => x.Name).ToList();
            }
        }
    }
}