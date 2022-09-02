namespace MoneyFox.Win;

using System.IO;
using Windows.Storage;
using Core.Common;
using Core.Interfaces;

internal class DbPathProvider : IDbPathProvider
{
    public string GetDbPath()
    {
        return Path.Combine(path1: ApplicationData.Current.LocalFolder.Path, path2: DatabaseConfiguration.DatabaseName);
    }
}
