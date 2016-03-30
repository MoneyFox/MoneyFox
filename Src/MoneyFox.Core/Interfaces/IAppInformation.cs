namespace MoneyFox.Core.Interfaces
{
    /// <summary>
    ///     Interface to access information of the app package.
    /// </summary>
    public interface IAppInformation
    {
        /// <summary>
        ///     Returns the ID of the current application
        /// </summary>
        string Id { get; }

        /// <summary>
        ///     Returns the version of the package.
        /// </summary>
        string Version { get; }
    }
}