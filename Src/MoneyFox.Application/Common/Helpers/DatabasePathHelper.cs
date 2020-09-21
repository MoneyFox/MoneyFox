using NLog;
using SQLitePCL;
using System;
using System.Globalization;
using System.IO;

namespace MoneyFox.Application.Common.Helpers
{
    public static class DatabasePathHelper
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        private const string DATABASE_NAME = "moneyfox3.db";

        public static string GetDbPath()
        {
            var databasePath = string.Empty;
            switch(ExecutingPlatform.Current)
            {
                case AppPlatform.iOS:
                    Batteries_V2.Init();
                    databasePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                                                "..",
                                                "Library",
                                                DATABASE_NAME);
                    break;

                case AppPlatform.Android:
                    databasePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), DATABASE_NAME);
                    break;

                case AppPlatform.UWP:
                    databasePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), DATABASE_NAME);
                    break;

                default:
                    throw new NotSupportedException("Platform not supported");
            }

            Logger.Debug(CultureInfo.CurrentCulture, "Database Path: {dbPath}", databasePath);

            return databasePath;
        }
    }
}
