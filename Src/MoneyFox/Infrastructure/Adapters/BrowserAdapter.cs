namespace MoneyFox.Mobile.Infrastructure.Adapters
{

    using System;
    using System.Threading.Tasks;
    using Core.Interfaces;

    public class BrowserAdapter : IBrowserAdapter
    {
        public async Task OpenWebsiteAsync(Uri uri)
        {
            await Browser.OpenAsync(uri: uri, launchMode: BrowserLaunchMode.External);
        }
    }

}
