using MoneyFox.Foundation.Interfaces;
using Xamarin.Essentials;

namespace MoneyFox.iOS
{
    /// <inheritdoc />
    public class AppInformation : IAppInformation
    {
        /// <inheritdoc />
        public string GetVersion() => AppInfo.VersionString;
    }