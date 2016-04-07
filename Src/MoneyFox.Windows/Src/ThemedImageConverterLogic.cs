using System;
using System.Collections.Generic;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media.Imaging;

namespace MoneyManager.Windows
{
    public static class ThemedImageConverterLogic
    {
        private static readonly Dictionary<string, BitmapImage> ImageCache = new Dictionary<string, BitmapImage>();

        public static BitmapImage GetImage(string path, bool negateResult = false)
        {
            if (string.IsNullOrEmpty(path))
            {
                return null;
            }

            var isDarkTheme = Application.Current.RequestedTheme == ApplicationTheme.Dark;

            if (negateResult)
            {
                isDarkTheme = !isDarkTheme;
            }

            BitmapImage result;
            path = "ms-appx:" + string.Format(path, isDarkTheme ? "dark" : "light");

            // Check if we already cached the image
            if (!ImageCache.TryGetValue(path, out result))
            {
                result = new BitmapImage(new Uri(path, UriKind.Absolute));
                ImageCache.Add(path, result);
            }

            return result;
        }
    }
}