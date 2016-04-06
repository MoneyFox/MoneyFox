using Android.App;
using MoneyFox.Shared.Interfaces;

namespace MoneyFox.Droid
{
    public class AppInformation : IAppInformation
    {
        public string Version
            => Application.Context.PackageManager.GetPackageInfo(Application.Context.PackageName, 0).VersionName;
    }
}