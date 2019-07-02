using MoneyFox.Presentation.Interfaces;

namespace MoneyFox.Droid
{
    public class DroidAppInformation : IAppInformation
    {
        public string GetVersion()
            => Android.App.Application.Context.PackageManager.GetPackageInfo(Android.App.Application.Context.PackageName, 0).VersionName;
    }
}