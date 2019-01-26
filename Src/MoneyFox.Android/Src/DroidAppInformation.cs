using Android.App;
using MoneyFox.ServiceLayer.Interfaces;

namespace MoneyFox.Droid
{
    public class DroidAppInformation : IAppInformation
    {
        public string GetVersion()
            => Application.Context.PackageManager.GetPackageInfo(Application.Context.PackageName, 0).VersionName;
    }
}