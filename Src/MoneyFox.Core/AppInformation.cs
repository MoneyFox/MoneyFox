using System.Globalization;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Store;
using MoneyFox.Core.Interfaces;

namespace MoneyFox.Core
{
    /// <summary>
    ///     Grants access to package information
    /// </summary>
    public class AppInformation : IAppInformation
    {
        /// <summary>
        ///     Returns the Id of the current App.
        /// </summary>
        public string Id => CurrentApp.AppId.ToString();

        /// <summary>
        ///     Reads the version from the AppManifest and returns it formated.
        /// </summary>
        public string Version
        {
            get
            {
                var version = Package.Current.Id.Version;

                return string.Format(
                    CultureInfo.InvariantCulture,
                    "{0}.{1}.{2}.{3}",
                    version.Major,
                    version.Minor,
                    version.Build,
                    version.Revision);
            }
        }
    }
}