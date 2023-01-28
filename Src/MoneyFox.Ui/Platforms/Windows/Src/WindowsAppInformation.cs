namespace MoneyFox.Ui.Platforms.Windows.Src;

using System.Globalization;
using Core.Common.Interfaces;
using global::Windows.ApplicationModel;

public class WindowsAppInformation : IAppInformation
{
    /// <summary>
    ///     Returns the version of the package.
    /// </summary>
    public string GetVersion
    {
        get
        {
            var version = Package.Current.Id.Version;

            return string.Format(
                provider: CultureInfo.InvariantCulture,
                format: "{0}.{1}.{2}.{3}",
                version.Major,
                version.Minor,
                version.Build,
                version.Revision);
        }
    }
}
