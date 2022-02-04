using MoneyFox.Core._Pending_.Common.Constants;
using MoneyFox.Core.Interfaces;
using System.IO;
using Windows.Storage;

namespace MoneyFox.Win.Infrastructure;

internal class DbPathProvider : IDbPathProvider
{
    public string GetDbPath()
    {
        return Path.Combine(ApplicationData.Current.LocalFolder.Path, DatabaseConstants.DATABASE_NAME);
    }
}