using Foundation;
using MoneyFox.Application.Common.Interfaces;

namespace MoneyFox.iOS
{
    /// <inheritdoc/>
    public class AppInformation : IAppInformation
    {
        /// <inheritdoc/>
        public string GetVersion => NSBundle.MainBundle.ObjectForInfoDictionary("CFBundleVersion").ToString();
    }
}
