namespace MoneyFox.Mobile.Infrastructure.Adapters
{
    using Core.Interfaces;
    using System;
    using System.Threading.Tasks;
    using Xamarin.Essentials;

    public class BrowserAdapter : IBrowserAdapter
    {
        public async Task OpenWebsiteAsync(Uri uri) => await Browser.OpenAsync(uri, BrowserLaunchMode.External);
    }
}