#nullable enable
namespace MoneyFox.Droid
{
    using Android.App;
    using Android.Content;
    using Core._Pending_.Common.Interfaces;

    public class DroidAppInformation : IAppInformation
    {
        public string GetVersion
        {
            get
            {
                Context? context = Application.Context;

                if(context == null)
                {
                    return string.Empty;
                }

                return context.PackageManager
                           ?.GetPackageInfo(context.PackageName ?? string.Empty, 0)
                           ?.VersionName
                       ?? string.Empty;
            }
        }
    }
}