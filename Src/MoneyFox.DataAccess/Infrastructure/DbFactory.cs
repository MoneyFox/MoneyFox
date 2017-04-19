using System;

namespace MoneyFox.DataAccess.Infrastructure
{
    public interface IDbFactory : IDisposable
    {
        ApplicationContext Init();
    }

    /// <summary>
    ///     Provides an ApplicationContext as Singleton
    /// </summary>
    public class DbFactory : Disposable, IDbFactory
    {
        ApplicationContext dbContext;

        /// <summary>
        ///     Initializes and ApplicationContext if not already one exists and returns it.
        /// </summary>
        /// <returns>Singleton Application Context</returns>
        public ApplicationContext Init()
        {
            return dbContext ?? (dbContext = new ApplicationContext());
        }

        protected override void DisposeCore()
        {
            dbContext?.Dispose();
        }
    }
}