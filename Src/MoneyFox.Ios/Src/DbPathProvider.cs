namespace MoneyFox.iOS.Src
{

    using Core.Interfaces;
    using SQLitePCL;
    using System;
    using System.IO;
    using Core.Common;

    public class DbPathProvider : IDbPathProvider
    {
        public string GetDbPath()
        {
            Batteries_V2.Init();
            return Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.Personal),
                DatabaseConfiguration.DatabaseName);
        }
    }
}