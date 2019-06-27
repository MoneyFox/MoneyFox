using MoneyFox.Foundation;
using NLog;
using System;
using System.IO;

namespace MoneyFox.DataLayer
{
    public static class DatabasePathHelper
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        private const string databaseName = "moneyfox3.db";

        public static string GetDbPath()
        {
            String databasePath = "";
            switch (ExecutingPlatform.Current)
            {
                case AppPlatform.iOS:
                    SQLitePCL.Batteries_V2.Init();
                    databasePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "..", "Library", databaseName); ;
                    break;
                case AppPlatform.Android:
                    databasePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), databaseName);
                    break;

                case AppPlatform.UWP:
                    databasePath = databaseName;
                    break;

                default:
                    throw new NotImplementedException("Platform not supported");
            }

            logger.Debug("Database Path: {dbPath}", databasePath);
            return databasePath;
        }
    }
}
