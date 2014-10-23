using Windows.ApplicationModel;

namespace MoneyManager.Business
{
    internal class Utilities
    {
        public static string GetVersion()
        {
            return new PackageVersion().Major.ToString() + new PackageVersion().Minor
                   + new PackageVersion().Revision;
        }
    }
}