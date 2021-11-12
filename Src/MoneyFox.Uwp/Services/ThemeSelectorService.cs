using Microsoft.AppCenter.Crashes;
using MoneyFox.Uwp.Helpers;
using System;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
using Windows.Storage;
using Windows.UI.Core;
using Windows.UI.Xaml;

#nullable enable
namespace MoneyFox.Uwp.Services
{
    public static class ThemeSelectorService
    {
        private const string SettingsKey = "AppBackgroundRequestedTheme";

        public static ElementTheme Theme { get; set; } = ElementTheme.Default;

        public static void Initialize() => Theme = LoadThemeFromSettings();

        public static async Task SetThemeAsync(ElementTheme theme)
        {
            Theme = theme;

            await SetRequestedThemeAsync();
            SaveThemeInSettings(Theme);
        }

        public static async Task SetRequestedThemeAsync()
        {
            foreach(var view in CoreApplication.Views)
            {
                await view.Dispatcher
                          .RunAsync(
                              CoreDispatcherPriority.Normal,
                              () =>
                              {
                                  if(Window.Current.Content is FrameworkElement frameworkElement)
                                  {
                                      try
                                      {
                                          frameworkElement.RequestedTheme = Theme;
                                      }
                                      catch(AccessViolationException ex)
                                      {
                                          Crashes.TrackError(ex);
                                      }
                                  }
                              });
            }
        }

        private static ElementTheme LoadThemeFromSettings()
        {
            var cacheTheme = ElementTheme.Default;
            var themeName = ApplicationData.Current.LocalSettings.Read<string>(SettingsKey);

            if(!string.IsNullOrEmpty(themeName))
            {
                Enum.TryParse(themeName, out cacheTheme);
            }

            return cacheTheme;
        }

        private static void SaveThemeInSettings(ElementTheme theme)
            => ApplicationData.Current.LocalSettings.SaveString(SettingsKey, theme.ToString());
    }
}