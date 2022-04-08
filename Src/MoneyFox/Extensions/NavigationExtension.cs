namespace MoneyFox.Extensions
{
    using Core._Pending_.Exceptions;
    using NLog;
    using System;
    using System.Threading.Tasks;
    using Serilog;
    using Xamarin.Essentials;
    using Xamarin.Forms;

    public static class NavigationExtension
    {
        public static Task GoToModalAsync(this Shell shell, string route)
        {
            try
            {
                if(!(Routing.GetOrCreateContent(route) is Page page))
                {
                    return Task.CompletedTask;
                }

                return shell.Navigation.PushModalAsync(
                    new NavigationPage(page)
                    {
                        BarBackgroundColor = Color.Transparent,
                        BarTextColor = GetCurrentForegroundColor()
                    });
            }
            catch(Exception ex)
            {
                var exception = new NavigationException($"Navigation to route {route} failed. ", ex);
                Log.Error(exception, "Error during navigation");
                throw exception;
            }
        }

        private static Color GetCurrentForegroundColor()
        {
            if(AppInfo.RequestedTheme == AppTheme.Dark)
            {
                Application.Current.Resources.TryGetValue("TextPrimaryColor_Dark", out var colorDark);
                return (Color) colorDark;
            }
            Application.Current.Resources.TryGetValue("TextPrimaryColor_Light", out var colorLight);
            return (Color)colorLight;
        }
    }
}
