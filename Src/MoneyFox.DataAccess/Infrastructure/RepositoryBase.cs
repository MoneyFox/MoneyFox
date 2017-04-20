using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace MoneyFox.DataAccess.Infrastructure
{
    /// <summary>
    ///     Provides base operations who are used on all repositories
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class RepositoryBase<T> where T : class
    {
        private ApplicationContext dataContext;
        private readonly DbSet<T> dbSet;

        protected RepositoryBase(IDbFactory dbFactory)
        {
            DbFactory = dbFactory;
            dbSet = dataContext.Set<T>();
        }

        /// <summary>
        ///     Factory to create an ApplicationContext
        /// </summary>
        protected IDbFactory DbFactory { get; }

        /// <summary>
        ///     Current ApplicationContex.
        /// </summary>
        protected ApplicationContext DbContext => dataContext ?? (dataContext = DbFactory.Init());

        #region Implementation

        /// <summary>
        ///     Creates a new entry for the entity.
        /// </summary>
        /// <param name="entity">Entity to create.</param>
        public virtual void Add(T entity)
        {
            dbSet.Add(entity);
        }

        /// <summary>
        ///     Updats an existing entity
        /// </summary>
        /// <param name="entity">Entity to update.</param>
        public virtual void Update(T entity)
        {
            dbSet.Attach(entity);
            dataContext.Entry(entity).State = EntityState.Modified;
        }

        /// <summary>
        ///     Deletes an existing entity.
        /// </summary>
        /// <param name="entity">Entity to delete.</param>
        public virtual void Delete(T entity)
        {
            dbSet.Remove(entity);
        }

        /// <summary>
        ///     Deletes an existing entity who maches the passed filter.
        /// </summary>
        /// <param name="where">Filter for items to delete.</param>
        public virtual void Delete(Expression<Func<T, bool>> where)
        {
            var objects = dbSet.Where(where).AsEnumerable();
            foreach (var obj in objects)
                dbSet.Remove(obj);
        }

        /// <summary>
        ///     Get an item by the id. Returns Null if no item is found.
        /// </summary>
        /// <param name="id">Id of the item.</param>
        /// <returns>selected Item.</returns>
        public virtual async Task<T> GetById(int id)
        {
            return await dbSet.FindAsync(id);
        }

        /// <summary>
        ///     Returns all Items of the database table
        /// </summary>
        /// <returns></returns>
        public virtual IEnumerable<T> GetAll()
        {
            return dbSet;
        }

        /// <summary>
        ///     Returns all Items who match the passed filter.
        /// </summary>
        /// <param name="where">Filter to select for.</param>
        /// <returns>Selected Items.</returns>
        public virtual IQueryable<T> GetMany(Expression<Func<T, bool>> where)
        {
            return dbSet.Where(where);
        }

        /// <summary>
        ///     Get the first item who matches the passed filter.
        ///     Returns null if no item matches.
        /// </summary>
        /// <param name="where">Filter to seleect for.</param>
        /// <returns>First item that matched.</returns>
        public async Task<T> Get(Expression<Func<T, bool>> where)
        {
            return await dbSet.Where(where).FirstOrDefaultAsync();
        }

        #endregion    }
    }
}