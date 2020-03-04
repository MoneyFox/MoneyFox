using Microsoft.UI.Xaml.Controls;
using NLog;
using Windows.UI.Xaml;

namespace MoneyFox.Uwp.Helpers
{
    public static class NavHelper
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        public static string GetNavigateTo(NavigationViewItem item)
        {
            var value = (string) item.GetValue(NavigateToProperty);
            logger.Info("Get Navigation value: {val}", value);

            return value;
        }

        public static void SetNavigateTo(NavigationViewItem item, string value)
        {
            logger.Info("Set Navigation value: {value}", value);
            item.SetValue(NavigateToProperty, value);
        }

        public static readonly DependencyProperty NavigateToProperty =
            DependencyProperty.RegisterAttached("NavigateTo", typeof(string), typeof(NavHelper), new PropertyMetadata(null));
    }
}
