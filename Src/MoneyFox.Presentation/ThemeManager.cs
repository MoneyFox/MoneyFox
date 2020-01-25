using System.Collections.Generic;
using MoneyFox.Application.Common;
using MoneyFox.Application.Common.Adapters;
using MoneyFox.Application.Common.Facades;
using MoneyFox.Presentation.Style;
using Xamarin.Forms;
using XF.Material.Forms;
using XF.Material.Forms.Resources;

namespace MoneyFox.Presentation
{
    public static class ThemeManager
    {
        /// <summary>
        ///     Changes the theme of the app.     Add additional switch cases for more themes you add to the app.     This
        ///     also updates the local key storage value for the selected theme.
        /// </summary>
        /// <param name="theme"></param>
        public static void ChangeTheme(AppTheme theme)
        {
            ICollection<ResourceDictionary> mergedDictionaries = Xamarin.Forms.Application.Current.Resources.MergedDictionaries;
            if (mergedDictionaries != null)
            {
                //Update local key value with the new theme you select.
                new SettingsFacade(new SettingsAdapter()).Theme = theme;

                switch (theme)
                {
                    case AppTheme.Light:
                        mergedDictionaries.Add(new LightTheme());
                        break;

                    case AppTheme.Dark:
                        mergedDictionaries.Add(new DarkTheme());
                        break;

                    default:
                        mergedDictionaries.Add(new LightTheme());
                        break;
                }

                Material.Init(Xamarin.Forms.Application.Current,
                              new MaterialConfiguration
                              {
                                  ColorConfiguration = new MaterialColorConfiguration
                                  {
                                      Background = (Color) Xamarin.Forms.Application.Current.Resources["WindowBackgroundColor"],
                                      Error = Color.FromHex("#B00020"),
                                      OnBackground = (Color) Xamarin.Forms.Application.Current.Resources["PrimaryFontColor"],
                                      OnError = Color.White,
                                      OnPrimary = (Color) Xamarin.Forms.Application.Current.Resources["PrimaryFontColor"],
                                      OnSecondary = (Color) Xamarin.Forms.Application.Current.Resources["PrimaryFontColor"],
                                      OnSurface = (Color) Xamarin.Forms.Application.Current.Resources["PrimaryFontColor"],
                                      Primary = (Color) Xamarin.Forms.Application.Current.Resources["PrimaryColor"],
                                      Secondary = (Color) Xamarin.Forms.Application.Current.Resources["SecondaryColor"],
                                      Surface = (Color) Xamarin.Forms.Application.Current.Resources["ListItemColor"]
                                  }
                              });

                ((NavigationPage) Xamarin.Forms.Application.Current.MainPage).BarBackgroundColor =
                    (Color) Xamarin.Forms.Application.Current.Resources["AppBarColor"];
                ((NavigationPage) Xamarin.Forms.Application.Current.MainPage).BarTextColor =
                    (Color) Xamarin.Forms.Application.Current.Resources["PrimaryFontColor"];
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
