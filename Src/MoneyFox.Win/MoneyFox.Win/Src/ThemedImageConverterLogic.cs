namespace MoneyFox.Win;

using System;
using System.Collections.Generic;
using System.Globalization;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Media.Imaging;

public static class ThemedImageConverterLogic
{
    private static readonly Dictionary<string, BitmapImage> ImageCache = new();

    public static BitmapImage? GetImage(string path, bool negateResult = false)
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

        path = $"ms-appx:{string.Format(provider: CultureInfo.InvariantCulture, format: path, arg0: isDarkTheme ? "dark" : "light")}";

        // Check if we already cached the image
        if (!ImageCache.TryGetValue(key: path, value: out var result))
        {
            result = new(new(uriString: path, uriKind: UriKind.Absolute));
            ImageCache.Add(key: path, value: result);
        }

        return result;
    }
}
