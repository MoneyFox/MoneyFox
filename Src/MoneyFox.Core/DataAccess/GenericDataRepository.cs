using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.Data.Entity;
using MoneyFox.Core.Interfaces;

namespace MoneyFox.Core.DataAccess
{
    /// <summary>
    ///     a collection of default operation to access the database.
    /// </summary>
    /// <typeparam name="T">Database table type.</typeparam>
    public class GenericDataRepository<T> : IGenericDataRepository<T> where T : class
    {
        /// <summary>
        ///     Loads all items from the database table inclusive the associated tables via navigation properties
        /// </summary>
        /// <param name="navigationProperties">Navigation Properties to load.</param>
        /// <returns>IList with all loaded items.</returns>
        public virtual IList<T> GetAll(params Expression<Func<T, object>>[] navigationProperties)
        {
            List<T> list;
            using (var context = new MoneyFoxDataContext())
            {
                IQueryable<T> dbQuery = context.Set<T>();

                //Apply eager loading
                dbQuery = navigationProperties.Aggregate(dbQuery,
                    (current, navigationProperty) => current.Include(navigationProperty));

                list = dbQuery
                    .AsNoTracking()
                    .ToList();
            }
            return list;
        }

        /// <summary>
        ///     Loads all items from the database table inclusive the associated tables via
        ///     navigation properties filtered by the filter expression.
        /// </summary>
        /// <param name="filter">Conditions to filter the data.</param>
        /// <param name="navigationProperties">Navigation Properties to load.</param>
        /// <returns>IList with all loaded items.</returns>
        public virtual IList<T> GetList(Expression<Func<T, bool>> filter,
            params Expression<Func<T, object>>[] navigationProperties)
        {
            List<T> list;
            using (var context = new MoneyFoxDataContext())
            {
                IQueryable<T> dbQuery = context.Set<T>();

                //Apply eager loading
                dbQuery = navigationProperties.Aggregate(dbQuery,
                    (current, navigationProperty) => current.Include(navigationProperty));

                list = dbQuery
                    .AsNoTracking()
                    .Where(filter)
                    .ToList();
            }
            return list;
        }

        /// <summary>
        ///     Loads a single item with associated data.
        /// </summary>
        /// <param name="filter">Conditions to filter the data.</param>
        /// <param name="navigationProperties">Navigation Properties to load.</param>
        /// <returns>Loaded item T.</returns>
        public virtual T GetSingle(Expression<Func<T, bool>> filter,
            params Expression<Func<T, object>>[] navigationProperties)
        {
            T item;
            using (var context = new MoneyFoxDataContext())
            {
                IQueryable<T> dbQuery = context.Set<T>();

                //Apply eager loading
                dbQuery = navigationProperties.Aggregate(dbQuery,
                    (current, navigationProperty) => current.Include(navigationProperty));

                item = dbQuery
                    .AsNoTracking() //Don't track any changes for the selected item
                    .Where(filter)
                    .FirstOrDefault(filter); //Apply filter clause
            }
            return item;
        }

        /// <summary>
        ///     Save items to the database.
        /// </summary>
        /// <param name="items">Items to save.</param>
        public void Add(params T[] items)
        {
            using (var context = new MoneyFoxDataContext())
            {
                foreach (var item in items)
                {
                    context.Entry(item).State = EntityState.Added;
                }
                context.SaveChanges();
            }
        }

        /// <summary>
        ///     Updates existing items to the database.
        /// </summary>
        /// <param name="items">Items to update.</param>
        public void Update(params T[] items)
        {
            using (var context = new MoneyFoxDataContext())
            {
                foreach (var item in items)
                {
                    context.Entry(item).State = EntityState.Modified;
                }
                context.SaveChanges();
            }
        }

        /// <summary>
        ///     Removes item from the database.
        /// </summary>
        /// <param name="items">Items to remove.</param>
        public void Delete(params T[] items)
        {
            using (var context = new MoneyFoxDataContext())
            {
                foreach (var item in items)
                {
                    context.Entry(item).State = EntityState.Deleted;
                }
                context.SaveChanges();
            }
        }
    }
}