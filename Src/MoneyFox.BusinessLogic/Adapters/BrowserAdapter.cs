using System;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace MoneyFox.BusinessLogic.Adapters
{
    public interface IBrowserAdapter
    {
        Task OpenWebsite(Uri uri);
    }

    public class BrowserAdapter : IBrowserAdapter
    {
        public async Task OpenWebsite(Uri uri)
        {
            await Browser.OpenAsync(uri, BrowserLaunchMode.External).ConfigureAwait(true);
        }
    }
}
