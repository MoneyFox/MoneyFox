using MoneyFox.Application.Common.Interfaces;

#nullable enable
namespace MoneyFox.Droid
{
    public class DroidAppInformation : IAppInformation
    {
        public string GetVersion
        {
            get
            {
                Android.Content.Context? context = Android.App.Application.Context;

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
