using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using EntityFramework.DbContextScope.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace MoneyFox.DataAccess.Repositories
{
    /// <summary>
    ///     Provides base operations who are used on all repositories
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class RepositoryBase<T> where T : class
    {
        private readonly IAmbientDbContextLocator ambientDbContextLocator;

        /// <summary>
        ///     Default Constructor
        /// </summary>
        /// <param name="ambientDbContextLocator">Locator to load the db context.</param>
        protected RepositoryBase(IAmbientDbContextLocator ambientDbContextLocator)
        {
            this.ambientDbContextLocator = ambientDbContextLocator;
        }

        protected ApplicationContext DbContext
        {
            get
            {
                var dbContext = ambientDbContextLocator.Get<ApplicationContext>();

                if (dbContext == null)
                    throw new InvalidOperationException("No ambient DbContext of type ApplicationContext found. This means that this repository method has been called outside of the scope of a DbContextScope. A repository must only be accessed within the scope of a DbContextScope, which takes care of creating the DbContext instances that the repositories need and making them available as ambient contexts. This is what ensures that, for any given DbContext-derived type, the same instance is used throughout the duration of a business transaction. To fix this issue, use IDbContextScopeFactory in your top-level business logic service method to create a DbContextScope that wraps the entire business transaction that your service method implements. Then access this repository within that scope. Refer to the comments in the IDbContextScope.cs file for more details.");

                return dbContext;
            }
        }

        /// <summary>
        ///     Attach Foreign-Entities in the DB-Context
        /// </summary>
        /// <param name="entity"></param>
        protected abstract void AttachForeign(T entity);

        #region Implementation

        /// <summary>
        ///     Creates a new entry for the entity.
        /// </summary>
        /// <param name="entity">Entity to create.</param>
        public virtual void Add(T entity)
        {
            AttachForeign(entity);
            DbContext.Set<T>().Add(entity);
        }

        /// <summary>
        ///     Updats an existing entity
        /// </summary>
        /// <param name="entity">Entity to update.</param>
        public virtual void Update(T entity)
        {
            AttachForeign(entity);
            DbContext.Set<T>().Attach(entity);
            DbContext.Entry(entity).State = EntityState.Modified;
        }

        /// <summary>
        ///     Deletes an existing entity.
        /// </summary>
        /// <param name="entity">Entity to delete.</param>
        public virtual void Delete(T entity)
        {
            DbContext.Set<T>().Remove(entity);
        }

        /// <summary>
        ///     Deletes an existing entity who maches the passed filter.
        /// </summary>
        /// <param name="where">Filter for items to delete.</param>
        public virtual void Delete(Expression<Func<T, bool>> where)
        {
            var dbSet = DbContext.Set<T>();
            var objects = dbSet.Where(where).AsEnumerable();
            foreach (var obj in objects)
            {
                dbSet.Remove(obj);
            }
        }

        /// <summary>
        ///     Get an item by the id. Returns Null if no item is found.
        /// </summary>
        /// <param name="id">Id of the item.</param>
        /// <returns>selected Item.</returns>
        public virtual async Task<T> GetById(int id)
        {
            return await DbContext.Set<T>().FindAsync(id);
        }

        /// <summary>
        ///     Get the first item who matches the passed filter.
        ///     Returns null if no item matches.
        /// </summary>
        /// <param name="where">Filter to seleect for.</param>
        /// <returns>First item that matched.</returns>
        public async Task<T> Get(Expression<Func<T, bool>> where)
        {
            return await DbContext.Set<T>().Where(where).FirstOrDefaultAsync();
        }

        /// <summary>
        ///     Returns all Items of the database table
        /// </summary>
        /// <returns></returns>
        public virtual IQueryable<T> GetAll()
        {
            return DbContext.Set<T>();
        }

        /// <summary>
        ///     Returns all Items who match the passed filter.
        /// </summary>
        /// <param name="where">Filter to select for.</param>
        /// <returns>Selected Items.</returns>
        public virtual IQueryable<T> GetMany(Expression<Func<T, bool>> where)
        {
            return DbContext.Set<T>().Where(where);
        }

        #endregion
    }
}