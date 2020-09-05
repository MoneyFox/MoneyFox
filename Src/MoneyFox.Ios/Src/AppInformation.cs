using Foundation;
using MoneyFox.Application.Common.Interfaces;

namespace MoneyFox.iOS.Src
{
    /// <inheritdoc/>
    public class AppInformation : IAppInformation
    {
        /// <inheritdoc/>
        public string GetVersion
        {
            get => NSBundle.MainBundle.ObjectForInfoDictionary("CFBundleVersion").ToString();
        }
    }
}
