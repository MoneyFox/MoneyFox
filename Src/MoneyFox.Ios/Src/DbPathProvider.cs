namespace MoneyFox.iOS.Src
{
    using Core._Pending_.Common.Constants;
    using Core.Interfaces;
    using SQLitePCL;
    using System;
    using System.IO;

    public class DbPathProvider : IDbPathProvider
    {
        public string GetDbPath()
        {
            Batteries_V2.Init();
            return Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.Personal),
                DatabaseConstants.DATABASE_NAME);
        }
    }
}