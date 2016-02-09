using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using MoneyManager.Foundation;
using MoneyManager.Foundation.Interfaces;
using MoneyManager.Foundation.Model;
using PropertyChanged;

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
                //Don't use insert or replace here, because it will always replace the first element
                if (itemToSave.Id == 0)
                {
                    db.Insert(itemToSave);
                    itemToSave.Id = db.Table<Category>().OrderByDescending(x => x.Id).First().Id;
                } else
                {
                    db.Update(itemToSave);
                }
            }
        }

        /// <summary>
        ///     DeleteItem an item from the database
        /// </summary>
        /// <param name="payment">Category to delete.</param>
        protected override void DeleteFromDatabase(Category payment)
        {
            using (var db = connectionCreator.GetConnection())
            {
                db.Delete(payment);
            }
        }

        /// <summary>
        ///     Loads a list of Categories from the database
        /// </summary>
        /// <param name="filter">>Filter expression</param>
        /// <returns>Loaded categories.</returns>
        protected override List<Category> GetListFromDb(Expression<Func<Category, bool>> filter)
        {
            using (var db = connectionCreator.GetConnection())
            {
                var listQuery = db.Table<Category>();

                if (filter != null)
                {
                    var compiledFilter = filter.Compile();
                    listQuery = listQuery.Where(x => compiledFilter(x));
                }

                return listQuery.OrderBy(x => x.Name).ToList();
            }
        }
    }
}