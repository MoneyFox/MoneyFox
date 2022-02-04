using MoneyFox.Core._Pending_.Common.Constants;
using MoneyFox.Core.Interfaces;
using System;
using System.IO;

namespace MoneyFox.Droid.Src
{
    public class DbPathProvider : IDbPathProvider
    {
        public string GetDbPath()
        {
            return Path.Combine(
                            Environment.GetFolderPath(Environment.SpecialFolder.Personal),
                            DatabaseConstants.DATABASE_NAME);
        }
    }
}