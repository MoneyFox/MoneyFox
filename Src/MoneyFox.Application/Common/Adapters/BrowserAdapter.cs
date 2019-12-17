using System;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace MoneyFox.Application.Common.Adapters
{
    public interface IBrowserAdapter
    {
        Task OpenWebsiteAsync(Uri uri);
    }

    public class BrowserAdapter : IBrowserAdapter
    {
        public async Task OpenWebsiteAsync(Uri uri)
        {
            await Browser.OpenAsync(uri, BrowserLaunchMode.External);
        }
    }
}
