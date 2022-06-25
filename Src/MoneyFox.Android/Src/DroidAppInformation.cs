namespace MoneyFox.Droid
{
    using Core.Common.Interfaces;
    using global::Android.App;

    public class DroidAppInformation : IAppInformation
    {
        public string GetVersion
        {
            get
            {
                var context = Application.Context;
                return context.PackageManager?.GetPackageInfo(packageName: context.PackageName ?? string.Empty, flags: 0)?.VersionName ?? string.Empty;
            }
        }
    }

}
