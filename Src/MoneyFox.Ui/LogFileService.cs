namespace MoneyFox.Ui;

internal static class LogFileService
{
    public static FileInfo? GetLatestLogFileInfo()
    {
        var logFilePaths = Directory.GetFiles(path: FileSystem.AppDataDirectory, searchPattern: "moneyfox*").OrderByDescending(x => x);
        var latestLogFile = logFilePaths.Select(logFilePath => new FileInfo(logFilePath)).MaxBy(fi => fi.LastWriteTime);

        return latestLogFile;
    }
}
