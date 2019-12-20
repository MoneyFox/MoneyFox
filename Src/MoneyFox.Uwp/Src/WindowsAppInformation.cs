using MoneyFox.Application.Common.Interfaces;
using System.Globalization;
using Windows.ApplicationModel;

namespace MoneyFox.Uwp.Src
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
