using System.Globalization;
using Windows.ApplicationModel;
using MoneyManager.Foundation.Interfaces;

namespace MoneyManager.Windows
{
    /// <summary>
    ///     Grants access to package information
    /// </summary>
    public class AppInformation : IAppInformation
    {
        /// <summary>
        ///     Reads the version from the AppManifest and returns it formated.
        /// </summary>
        public string GetVersion
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