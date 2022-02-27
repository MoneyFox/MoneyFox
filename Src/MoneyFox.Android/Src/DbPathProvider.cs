namespace MoneyFox.Droid.Src
{
    using Core._Pending_.Common.Constants;
    using Core.Interfaces;
    using System;
    using System.IO;

    public class DbPathProvider : IDbPathProvider
    {
        public string GetDbPath() =>
            Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.Personal),
                DatabaseConstants.DATABASE_NAME);
    }
}