namespace MoneyFox.Core._Pending_.Common.Interfaces
{
    /// <summary>
    ///     Interface to access information of the app package.
    /// </summary>
    public interface IAppInformation
    {
        /// <summary>
        ///     Returns the version.
        /// </summary>
        string GetVersion { get; }
    }
}