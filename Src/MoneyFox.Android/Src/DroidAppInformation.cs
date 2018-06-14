using MoneyFox.Foundation.Interfaces;
using Xamarin.Essentials;

namespace MoneyFox.Droid
{
    public class DroidAppInformation : IAppInformation
    {
        public string GetVersion() => AppInfo.VersionString;
    }
}