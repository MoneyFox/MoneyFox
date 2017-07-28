namespace MoneyFox.Foundation.Interfaces
{
    /// <summary>
    ///     Provides functions to set the theme of the app.
    /// </summary>
    public interface IThemeService
    {
        /// <summary>
        ///     Gets the currently set theme.
        /// </summary>
        AppTheme GetActiveTheme();

        /// <summary>
        ///     Sets the passed theme.
        /// </summary>
        /// <param name="theme">Theme to set.</param>
        void SetTheme(AppTheme theme);
    }
}
