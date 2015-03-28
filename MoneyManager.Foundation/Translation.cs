using Windows.ApplicationModel.Resources;

namespace MoneyManager.Foundation {
    public class Translation {
        public static string GetTranslation(string key) {
            return ResourceLoader.GetForViewIndependentUse().GetString(key);
        }
    }
}