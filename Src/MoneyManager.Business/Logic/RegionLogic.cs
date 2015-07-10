using System.Collections.Generic;
using System.Linq;
using Windows.Globalization;
using Windows.System.UserProfile;

namespace MoneyManager.Business.Logic
{
    public class RegionLogic
    {
        public static List<string> GetSupportedLanguages()
        {
            return GlobalizationPreferences.Languages.ToList();
        }

        public static void SetPrimaryLanguage(string lang)
        {
            ApplicationLanguages.PrimaryLanguageOverride = lang;
        }

        public static string GetPrimaryLanguage()
        {
            return ApplicationLanguages.Languages.First();
        }
    }
}