namespace MoneyFox.Droid
{

    using System;
    using System.IO;
    using MoneyFox.Core.Common;
    using MoneyFox.Core.Interfaces;

    // TODO Is this still needed?
    public class DbPathProvider : IDbPathProvider
    {
        public string GetDbPath()
        {
            return Path.Combine(path1: Environment.GetFolderPath(Environment.SpecialFolder.Personal), path2: DatabaseConfiguration.DatabaseName);
        }
    }

}
