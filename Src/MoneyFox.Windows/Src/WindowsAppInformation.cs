using MoneyFox.Foundation.Interfaces;
using Xamarin.Essentials;

namespace MoneyFox.Windows
{
    public class WindowsAppInformation : IAppInformation
    {
        /// <summary>
        ///     Returns the version of the package.
        /// </summary>
        public string GetVersion() => AppInfo.VersionString;
    }
}
