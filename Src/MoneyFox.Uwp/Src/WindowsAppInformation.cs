using System.Globalization;
using Windows.ApplicationModel;
using MoneyFox.Presentation.Interfaces;

namespace MoneyFox.Uwp
{
    public class WindowsAppInformation : IAppInformation
    {
        /// <summary>
        ///     Returns the version of the package.
        /// </summary>
        public string GetVersion()
        {
            PackageVersion version = Package.Current.Id.Version;

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
