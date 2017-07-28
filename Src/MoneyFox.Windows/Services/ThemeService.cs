using Windows.UI.Xaml;
using MoneyFox.Foundation;
using MoneyFox.Foundation.Interfaces;

namespace MoneyFox.Windows.Services
{
    /// <inheritdoc />
    public class ThemeService : IThemeService
    {
        private readonly ISettingsManager settingsManager;

        /// <summary>
        ///     Constructor
        /// </summary>
        public ThemeService(ISettingsManager settingsManager)
        {
            this.settingsManager = settingsManager;
        }

        /// <inheritdoc />
        public AppTheme GetActiveTheme()
        {
            return settingsManager.Theme;
        }

        /// <inheritdoc />
        public void SetTheme(AppTheme theme)
        {
            var frameworkElement = Window.Current.Content as FrameworkElement;
            if (frameworkElement != null)
            {
                frameworkElement.RequestedTheme = GetTheme(theme);
            }
        }

        private ElementTheme GetTheme(AppTheme theme)
        {
            switch (theme)
            {
                case AppTheme.Dark:
                    return ElementTheme.Dark;
                case AppTheme.Light:
                    return ElementTheme.Light;
                case AppTheme.System:
                default:
                    return ElementTheme.Default;
            }
        }
    }
}
