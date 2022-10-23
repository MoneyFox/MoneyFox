namespace MoneyFox.Ui.Platforms.iOS.Src;

using Core.Common.Interfaces;
using Foundation;

/// <inheritdoc />
public class AppInformation : IAppInformation
{
    /// <inheritdoc />
    public string GetVersion => NSBundle.MainBundle.ObjectForInfoDictionary("CFBundleVersion").ToString();
}
