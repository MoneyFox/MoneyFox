using Microsoft.AppCenter.Crashes;
using MoneyFox.Uwp.Helpers;
using System;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
using Windows.Storage;
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.ViewManagement;
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
            foreach(CoreApplicationView view in CoreApplication.Views)
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

                                          Color color;
                                          if(Theme == ElementTheme.Default)
                                          {
                                              color = ConvertColorFromHexString(App.Current
                                                  .Resources["ColorCustomBackground"]
                                                  .ToString());
                                          }
                                          else
                                          {
                                              color = Theme == ElementTheme.Dark
                                                  ? ConvertColorFromHexString("#FF121212")
                                                  : ConvertColorFromHexString("#FFF0F3F9");
                                          }

                                          //remove the solid-colored backgrounds behind the caption controls and system back button
                                          ApplicationViewTitleBar viewTitleBar = ApplicationView.GetForCurrentView().TitleBar;
                                          viewTitleBar.BackgroundColor = color;
                                          viewTitleBar.ButtonBackgroundColor = color;
                                          viewTitleBar.InactiveBackgroundColor = color;
                                          viewTitleBar.ButtonInactiveBackgroundColor = color;
                                      }
                                      catch(AccessViolationException ex)
                                      {
                                          Crashes.TrackError(ex);
                                      }
                                  }
                              });
            }
        }
        
        private static Color ConvertColorFromHexString(string hexString)
        {
            var colorStr = hexString.Replace("#", string.Empty);

            var a = (byte)Convert.ToUInt32(colorStr.Substring(0, 2), 16);
            var r = (byte)Convert.ToUInt32(colorStr.Substring(2, 2), 16);
            var g = (byte)Convert.ToUInt32(colorStr.Substring(4, 2), 16);
            var b = (byte)Convert.ToUInt32(colorStr.Substring(6, 2), 16);

            return Color.FromArgb(a, r, g, b);
        }

        private static ElementTheme LoadThemeFromSettings()
        {
            var cacheTheme = ElementTheme.Default;
            string themeName = ApplicationData.Current.LocalSettings.Read<string>(SettingsKey);

            if(!string.IsNullOrEmpty(themeName))
            {
                Enum.TryParse(themeName, out cacheTheme);
            }

            return cacheTheme;
        }

        private static void SaveThemeInSettings(ElementTheme theme) =>
            ApplicationData.Current.LocalSettings.SaveString(SettingsKey, theme.ToString());
    }
}