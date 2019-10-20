using System;
using System.Globalization;
using System.Resources;

namespace MoneyFox.Presentation.Utilities
{
    /// <summary>
    ///     Provides Access to the localized Resources.
    /// </summary>
    public class LocalizedResources
    {
        private const string DEFAULT_LANGUAGE = "en-EN";

        private readonly ResourceManager resourceManager;
        private readonly CultureInfo currentCultureInfo;

        public string this[string key] => resourceManager.GetString(key, currentCultureInfo) ?? key;

        public LocalizedResources(Type resource, string language = null)
            : this(resource, new CultureInfo(language ?? DEFAULT_LANGUAGE))
        {
        }

        public LocalizedResources(Type resource, CultureInfo cultureInfo)
        {
            currentCultureInfo = cultureInfo;
            resourceManager = new ResourceManager(resource);
        }
    }
}
