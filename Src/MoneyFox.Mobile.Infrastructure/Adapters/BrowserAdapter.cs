using MoneyFox.Application.Common.Adapters;
using System;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace MoneyFox.Mobile.Infrastructure.Adapters
{
    public class BrowserAdapter : IBrowserAdapter
    {
        public async Task OpenWebsiteAsync(Uri uri) => await Browser.OpenAsync(uri, BrowserLaunchMode.External);
    }
}