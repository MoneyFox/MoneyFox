using System;
using Windows.ApplicationModel.Resources;

namespace MoneyTracker.Src
{
    public class Utilities
    {
        public static string GetTranslation(string text)
        {
            return ResourceLoader.GetForCurrentView().GetString(text);
        }

        public static int GetMaxId()
        {
            var roamingSettings = Windows.Storage.ApplicationData.Current.RoamingSettings;
            return roamingSettings.Values.Count;
        }
    }
}