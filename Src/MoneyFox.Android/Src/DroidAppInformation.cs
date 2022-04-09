namespace MoneyFox.Droid
{

    using Android.App;
    using Core.Common.Interfaces;

    public class DroidAppInformation : IAppInformation
    {
        public string GetVersion
        {
            get
            {
                var context = Application.Context;
                if (context == null)
                {
                    return string.Empty;
                }

                return context.PackageManager?.GetPackageInfo(packageName: context.PackageName ?? string.Empty, flags: 0)?.VersionName ?? string.Empty;
            }
        }
    }

}
