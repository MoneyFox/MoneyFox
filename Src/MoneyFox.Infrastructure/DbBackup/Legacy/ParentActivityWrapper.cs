namespace MoneyFox.Infrastructure.DbBackup.Legacy;

public static class ParentActivityWrapper
{
    /// <summary>
    ///     The view, window or activity from where the interactive login happens. This is required in Android     to
    ///     capture authentication result from the browser.
    /// </summary>
    /// <remarks>
    ///     Since this is a shared project, there is no reference to the Activity type, so keep this as an object.
    /// </remarks>
    public static object? ParentActivity { get; set; }
}
