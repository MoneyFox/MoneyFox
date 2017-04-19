using System;

namespace MoneyFox.DataAccess.Infrastructure
{
    /// <summary>
    ///     Provides an ApplicationContext as Singleton
    /// </summary>
    public interface IDbFactory : IDisposable
    {
        ApplicationContext Init();
    }
}