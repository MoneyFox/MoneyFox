namespace MoneyFox.Ui.Platforms.Windows.Src;

using Core.Common;
using Core.Interfaces;
using global::Windows.Storage;

internal class DbPathProvider : IDbPathProvider
{
    public string GetDbPath()
    {
        return Path.Combine(path1: ApplicationData.Current.LocalFolder.Path, path2: DatabaseConfiguration.DatabaseName);
    }
}
