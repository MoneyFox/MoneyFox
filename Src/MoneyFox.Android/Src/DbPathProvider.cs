namespace MoneyFox.Droid.Src
{

    using Core.Interfaces;
    using System;
    using System.IO;
    using Core.Common;

    public class DbPathProvider : IDbPathProvider
    {
        public string GetDbPath() =>
            Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.Personal),
                DatabaseConfiguration.DatabaseName);
    }
}
