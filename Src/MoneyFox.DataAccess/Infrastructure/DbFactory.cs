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
        Task<ApplicationContext> Init();
    }

    /// <summary>
    ///     Provides an ApplicationContext as Singleton
    /// </summary>
    public class DbFactory : Disposable, IDbFactory
    {
        ApplicationContext dbContext;

        /// <summary>
        ///     Migrates the database and initializes and ApplicationContext if not already one exists and returns it.
        /// </summary>
        /// <returns>Singleton Application Context</returns>
        public async Task<ApplicationContext> Init()
        {
            if (dbContext == null)
            {
                dbContext = new ApplicationContext();
            }
            await dbContext.Database.MigrateAsync();
            return dbContext;
        }

        protected override void DisposeCore()
        {
            dbContext?.Dispose();
        }
    }
}