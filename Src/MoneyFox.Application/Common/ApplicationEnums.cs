namespace MoneyFox.Application.Common
{
    public enum AppTheme
    {
        Dark,
        Light
    }

    public enum AppPlatform
    {
        Android,
        iOS,
        UWP
    }

    /// <summary>
    ///     Indicates if a backup was started by an automatic task or by the user.
    /// </summary>
    public enum BackupMode
    {
        Manual,
        Automatic
    }
}
