using MoneyFox.Application.Common.Interfaces;

namespace MoneyFox.Droid.Src
{
    public class DroidAppInformation : IAppInformation
    {
        public string GetVersion()
        {
            return Android.App.Application.Context.PackageManager.GetPackageInfo(Android.App.Application.Context.PackageName, 0)
                                                  .VersionName;
        }
    }
}
