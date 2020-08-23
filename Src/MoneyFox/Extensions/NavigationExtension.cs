using System.Threading.Tasks;
using Xamarin.Forms;

namespace MoneyFox.Extensions
{
    public static class NavigationExtension
    {
        public static Task GoToModalAsync(this Shell shell, string route)
        {
            var page = Routing.GetOrCreateContent(route) as Page;

            if(page is null)
                return Task.CompletedTask;

            return shell.Navigation.PushModalAsync(new NavigationPage(page)
            {
                BarBackgroundColor = Color.Transparent
            });
        }
    }
}
