namespace MoneyFox.Common.Extensions
{

    using System;
    using System.Threading.Tasks;
    using Core.ApplicationCore.Domain.Exceptions;
    using Serilog;
    using Xamarin.Essentials;
    using Xamarin.Forms;

    public static class NavigationExtension
    {
        public static Task GoToModalAsync(this Shell shell, string route)
        {
            try
            {
                if (!(Routing.GetOrCreateContent(route) is Page page))
                {
                    return Task.CompletedTask;
                }

                return shell.Navigation.PushModalAsync(
                    new NavigationPage(page) { BarBackgroundColor = Color.Transparent, BarTextColor = GetCurrentForegroundColor() });
            }
            catch (Exception ex)
            {
                var exception = new NavigationException(message: $"Navigation to route {route} failed. ", innerException: ex);
                Log.Error(exception: exception, messageTemplate: "Error during navigation");

                throw exception;
            }
        }

        private static Color GetCurrentForegroundColor()
        {
            if (AppInfo.RequestedTheme == AppTheme.Dark)
            {
                Application.Current.Resources.TryGetValue(key: "TextPrimaryColor_Dark", value: out var colorDark);

                return (Color)colorDark;
            }

            Application.Current.Resources.TryGetValue(key: "TextPrimaryColor_Light", value: out var colorLight);

            return (Color)colorLight;
        }
    }

}
