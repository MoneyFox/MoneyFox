using System;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace MoneyFox.BusinessLogic.Adapters
{
    /// <summary>
    ///     Adapter to the browser logic.
    /// </summary>
    public interface IBrowserAdapter
    {
        Task OpenWebsite(Uri uri);
    }

    /// <inheritdoc />
    public class BrowserAdapter : IBrowserAdapter
    {
        public async Task OpenWebsite(Uri uri)
        {
            await Browser.OpenAsync(uri, BrowserLaunchMode.External).ConfigureAwait(false);
        }
    }
}
