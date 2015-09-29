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
    public class CategoryDataAccess : AbstractDataAccess<Category>
    {
        private readonly ISqliteConnectionCreator connectionCreator;

        public CategoryDataAccess(ISqliteConnectionCreator connectionCreator)
        {
            this.connectionCreator = connectionCreator;
        }

        /// <summary>
        ///     Saves an Category to database
        /// </summary>
        /// <param name="itemToSave">Category to save.</param>
        protected override void SaveToDb(Category itemToSave)
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
        ///     DeleteItem an item from the database
        /// </summary>
        /// <param name="category">Category to delete.</param>
        protected override void DeleteFromDatabase(Category category)
        {
            using (var dbConn = connectionCreator.GetConnection())
            {
                dbConn.Delete(category);
            }
        }

        /// <summary>
        ///     Loads a list of Categories from the database
        /// </summary>
        /// <param name="filter">>Filter expression</param>
        /// <returns>Loaded categories.</returns>
        protected override List<Category> GetListFromDb(Expression<Func<Category, bool>> filter)
        {
            using (var dbConn = connectionCreator.GetConnection())
            {
                return dbConn.Table<Category>()
                    .OrderBy(x => x.Name)
                    .ToList();
            }
        }
    }
}