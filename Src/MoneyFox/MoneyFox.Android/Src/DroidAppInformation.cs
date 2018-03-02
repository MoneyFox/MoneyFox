using Android.App;
using MoneyFox.Foundation.Interfaces;

namespace MoneyFox.Droid.Src
{
    public class DroidAppInformation : IAppInformation
    {
        public string GetVersion()
            => Application.Context.PackageManager.GetPackageInfo(Application.Context.PackageName, 0).VersionName;
    }
}