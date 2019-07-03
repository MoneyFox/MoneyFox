using MoneyFox.Application;
using NLog;
using System;
using System.Globalization;
using System.IO;

namespace MoneyFox.DataLayer
{
    public static class DatabasePathHelper
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        private const string DATABASE_NAME = "moneyfox3.db";

        public static string GetDbPath()
        {
            string databasePath = "";
            switch (ExecutingPlatform.Current)
            {
                case AppPlatform.iOS:
                    SQLitePCL.Batteries_V2.Init();
                    databasePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "..", "Library", DATABASE_NAME);
                    break;
                case AppPlatform.Android:
                    databasePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), DATABASE_NAME);
                    break;

                case AppPlatform.UWP:
                    databasePath = DATABASE_NAME;
                    break;

                default:
                    throw new NotSupportedException("Platform not supported");
            }

            Logger.Debug(CultureInfo.CurrentCulture, "Database Path: {dbPath}", databasePath);
            return databasePath;
        }
    }
}
