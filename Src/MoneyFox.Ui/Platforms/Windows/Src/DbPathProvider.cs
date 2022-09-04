namespace MoneyFox.Ui.Platforms.Windows.Src;

using global::Windows.Storage;
using MoneyFox.Core.Common;
using MoneyFox.Core.Interfaces;

internal class DbPathProvider : IDbPathProvider
{
    public string GetDbPath()
    {
        return Path.Combine(path1: ApplicationData.Current.LocalFolder.Path, path2: DatabaseConfiguration.DatabaseName);
    }
}
