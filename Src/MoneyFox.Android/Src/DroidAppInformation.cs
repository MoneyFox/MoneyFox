using Android.Content;
using MoneyFox.Core._Pending_.Common.Interfaces;

#nullable enable
namespace MoneyFox.Droid
{
    public class DroidAppInformation : IAppInformation
    {
        public string GetVersion
        {
            get
            {
                Context? context = Android.App.Application.Context;

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