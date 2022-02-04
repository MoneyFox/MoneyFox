using MoneyFox.Core._Pending_.Common.Constants;
using MoneyFox.Core.Interfaces;
using SQLitePCL;
using System;
using System.IO;

namespace MoneyFox.iOS.Src
{
    public class DbPathProvider : IDbPathProvider
    {
        public string GetDbPath()
        {
            Batteries_V2.Init();
            return Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                "..",
                "Library",
                DatabaseConstants.DATABASE_NAME);
        }
    }
}