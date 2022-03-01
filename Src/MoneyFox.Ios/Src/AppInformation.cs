namespace MoneyFox.iOS
{
    using Core._Pending_.Common.Interfaces;
    using Foundation;

    /// <inheritdoc />
    public class AppInformation : IAppInformation
    {
        /// <inheritdoc />
        public string GetVersion => NSBundle.MainBundle.ObjectForInfoDictionary("CFBundleVersion").ToString();
    }
}