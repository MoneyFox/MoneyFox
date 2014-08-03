using Windows.ApplicationModel.Resources;

namespace MoneyManager.Src
{
    public class Utilities
    {
        public static string GetTranslation(string text)
        {
            return ResourceLoader.GetForViewIndependentUse().GetString(text);
        }

        public static int GetMaxId()
        {
            var roamingSettings = Windows.Storage.ApplicationData.Current.RoamingSettings;
            return roamingSettings.Values.Count;
        }
    }
}