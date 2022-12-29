namespace MoneyFox.Ui.Infrastructure.Adapters;

using Core.Interfaces;

public class BrowserAdapter : IBrowserAdapter
{
    public async Task OpenWebsiteAsync(Uri uri)
    {
        await Browser.Default.OpenAsync(uri: uri, launchMode: BrowserLaunchMode.External);
    }
}
