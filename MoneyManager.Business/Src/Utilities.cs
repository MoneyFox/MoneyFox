
using Windows.ApplicationModel;
using Windows.Storage;

namespace MoneyManager.Business.Src
{
    internal class Utilities
    {
        public static string GetVersion()
        {
            return new PackageVersion().Major.ToString() + new PackageVersion().Minor 
                + new PackageVersion().Revision;
        }

        public static int GetMaxId()
        {
            ApplicationDataContainer roamingSettings = ApplicationData.Current.RoamingSettings;
            return roamingSettings.Values.Count;
        }
    }
}