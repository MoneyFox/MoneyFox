namespace MoneyFox.Ui.Platforms.MacCatalyst.Src;

using Core.Common;
using Core.Interfaces;
using SQLitePCL;

public class DbPathProvider : IDbPathProvider
{
    public string GetDbPath()
    {
        Batteries_V2.Init();

        return Path.Combine(path1: Environment.GetFolderPath(Environment.SpecialFolder.Personal), path2: DatabaseConfiguration.DatabaseName);
    }
}
