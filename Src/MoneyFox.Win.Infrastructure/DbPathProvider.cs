namespace MoneyFox.Win.Infrastructure;

using Core.Interfaces;
using System.IO;
using Windows.Storage;
using Core.Common;

internal class DbPathProvider : IDbPathProvider
{
    public string GetDbPath() =>
        Path.Combine(ApplicationData.Current.LocalFolder.Path, DatabaseConfiguration.DatabaseName);
}