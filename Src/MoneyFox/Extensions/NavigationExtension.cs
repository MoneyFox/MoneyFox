using MoneyFox.Core._Pending_.Exceptions;
using NLog;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace MoneyFox.Extensions
{
    public static class NavigationExtension
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        public static Task GoToModalAsync(this Shell shell, string route)
        {
            try
            {
                if(!(Routing.GetOrCreateContent(route) is Page page))
                {
                    return Task.CompletedTask;
                }

                return shell.Navigation.PushModalAsync(
                    new NavigationPage(page) { BarBackgroundColor = Color.Transparent });
            }
            catch(Exception ex)
            {
                var exception = new NavigationException($"Navigation to route {route} failed. ", ex);
                logger.Error(exception);
                throw exception;
            }
        }
    }
}