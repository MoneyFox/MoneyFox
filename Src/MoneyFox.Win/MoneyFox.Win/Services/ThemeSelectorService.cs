using Microsoft.AppCenter.Crashes;
using Microsoft.UI.Xaml;
using System;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
using Windows.Storage;
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.ViewManagement;

namespace MoneyFox.Win.Services
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

                                    string? colorString;
                                    if(Theme == ElementTheme.Default)
                                    {
                                        colorString = Application.Current.Resources["ColorCustomBackground"].ToString();
                                    }
                                    else
                                    {
                                        colorString = Theme == ElementTheme.Dark
                                            ? "#FF121212"
                                            : "#FFF0F3F9";
                                    }

                                    if(colorString == null)
                                    {
                                        return;
                                    }

                                    var color = ConvertColorFromHexString(colorString);

                                    //remove the solid-colored backgrounds behind the caption controls and system back button
                                    var titleBar = ApplicationView.GetForCurrentView().TitleBar;
                                    titleBar.BackgroundColor = color;
                                    titleBar.ButtonBackgroundColor = color;
                                    titleBar.InactiveBackgroundColor = color;
                                    titleBar.ButtonInactiveBackgroundColor = color;
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
            string colorStr = hexString.Replace("#", string.Empty);

            byte a = (byte)Convert.ToUInt32(colorStr.Substring(0, 2), 16);
            byte r = (byte)Convert.ToUInt32(colorStr.Substring(2, 2), 16);
            byte g = (byte)Convert.ToUInt32(colorStr.Substring(4, 2), 16);
            byte b = (byte)Convert.ToUInt32(colorStr.Substring(6, 2), 16);

            return Color.FromArgb(a, r, g, b);
        }

        private static ElementTheme LoadThemeFromSettings()
        {
            var cacheTheme = ElementTheme.Default;
            string themeName = ApplicationData.Current.LocalSettings.Values[SettingsKey].ToString();

            if(!string.IsNullOrEmpty(themeName))
            {
                Enum.TryParse(themeName, out cacheTheme);
            }

            return cacheTheme;
        }

        private static void SaveThemeInSettings(ElementTheme theme) =>
            ApplicationData.Current.LocalSettings.Values.Add(SettingsKey, theme.ToString());
    }
}