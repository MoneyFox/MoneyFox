using System;
using System.IO;

namespace MoneyFox.Application.Common
{
    public static class LogPathHelper
    {
        private const string LOGFILE_NAME = "moneyfox.log";

        public static string GetLogPath()
        {
            switch (ExecutingPlatform.Current)
            {
                case AppPlatform.iOS:
                    string docFolder = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
                    string libFolder = Path.Combine(docFolder, "..", "Library", "Databases");

                    if (!Directory.Exists(libFolder)) Directory.CreateDirectory(libFolder);

                    return Path.Combine(libFolder, LOGFILE_NAME);

                case AppPlatform.Android:
                    return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), LOGFILE_NAME);

                case AppPlatform.UWP:
                    return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), LOGFILE_NAME);

                default:
                    throw new NotSupportedException("Platform not supported");
            }
        }
    }
}
