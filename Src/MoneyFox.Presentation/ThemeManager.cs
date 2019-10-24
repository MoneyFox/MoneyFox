using System.Collections.Generic;
using MoneyFox.Application;
using MoneyFox.Application.Adapters;
using MoneyFox.Presentation.Facades;
using MoneyFox.Presentation.Style;
using Xamarin.Forms;

namespace MoneyFox.Presentation
{
    public static class ThemeManager
    {
        /// <summary>
        ///     Changes the theme of the app.
        ///     Add additional switch cases for more themes you add to the app.
        ///     This also updates the local key storage value for the selected theme.
        /// </summary>
        /// <param name="theme"></param>
        public static void ChangeTheme(AppTheme theme)
        {
            ICollection<ResourceDictionary> mergedDictionaries = Xamarin.Forms.Application.Current.Resources.MergedDictionaries;
            if (mergedDictionaries != null)
            {
                mergedDictionaries.Clear();

                //Update local key value with the new theme you select.
                new SettingsFacade(new SettingsAdapter()).Theme = theme;

                switch (theme)
                {
                    case AppTheme.Light:
                    {
                        mergedDictionaries.Add(new LightTheme());

                        break;
                    }

                    case AppTheme.Dark:
                    {
                        mergedDictionaries.Add(new DarkTheme());

                        break;
                    }

                    default:
                        mergedDictionaries.Add(new LightTheme());

                        break;
                }
            }
        }

        /// <summary>
        ///     Reads current theme id from the local storage and loads it.
        /// </summary>
        public static void LoadTheme()
        {
            AppTheme currentTheme = CurrentTheme();
            ChangeTheme(currentTheme);
        }

        /// <summary>
        ///     Gives current/last selected theme from the local storage.
        /// </summary>
        /// <returns></returns>
        public static AppTheme CurrentTheme()
        {
            return new SettingsFacade(new SettingsAdapter()).Theme;
        }
    }
}
