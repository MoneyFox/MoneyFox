using Android.App;
using MoneyManager.Foundation.OperationContracts;

namespace MoneyManager.Droid
{
    public class AppInformation : IAppInformation
    {
        public string GetVersion => Application.Context.PackageManager.GetPackageInfo(Application.Context.PackageName, 0).VersionName;
    }
}