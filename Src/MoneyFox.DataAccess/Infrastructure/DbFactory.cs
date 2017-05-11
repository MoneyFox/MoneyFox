using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace MoneyFox.DataAccess.Infrastructure
{
    /// <summary>
    ///     Provides an ApplicationContext as Singleton
    /// </summary>
    public interface IDbFactory : IDisposable
    {
        /// <summary>
        ///     Migrates the database and initializes and ApplicationContext if not already one exists and returns it.
        /// </summary>
        /// <returns>Singleton Application Context</returns>
        Task<ApplicationContext> Init();
    }

    /// <inheritdoc />
    public class DbFactory : Disposable, IDbFactory
    {
        private ApplicationContext dbContext;

        /// <inheritdoc />
        public async Task<ApplicationContext> Init()
        {
            if (dbContext == null)
            {
                dbContext = new ApplicationContext();
            }
            await dbContext.Database.MigrateAsync();
            return dbContext;
        }

        /// <summary>
        ///     Dispose the current DbFactory
        /// </summary>
        protected override void DisposeCore()
        {
            dbContext?.Dispose();
        }
    }
}