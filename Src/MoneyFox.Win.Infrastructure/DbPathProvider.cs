namespace MoneyFox.Win.Infrastructure;

using Core._Pending_.Common.Constants;
using Core.Interfaces;
using System.IO;
using Windows.Storage;

internal class DbPathProvider : IDbPathProvider
{
    public string GetDbPath() =>
        Path.Combine(ApplicationData.Current.LocalFolder.Path, DatabaseConstants.DATABASE_NAME);
}