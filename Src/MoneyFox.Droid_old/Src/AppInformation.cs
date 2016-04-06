using Android.App;
using MoneyManager.Foundation.Interfaces;

namespace MoneyManager.Droid
{
    public class AppInformation : IAppInformation
    {
        public string Version
            => Application.Context.PackageManager.GetPackageInfo(Application.Context.PackageName, 0).VersionName;
    }
}