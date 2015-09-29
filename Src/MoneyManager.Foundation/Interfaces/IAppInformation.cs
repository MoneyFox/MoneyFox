namespace MoneyManager.Foundation.Interfaces
{
    /// <summary>
    ///     Interface to access information of the app package.
    /// </summary>
    public interface IAppInformation
    {
        /// <summary>
        ///     Returns the version of the package.
        /// </summary>
        string GetVersion { get; }
    }
}