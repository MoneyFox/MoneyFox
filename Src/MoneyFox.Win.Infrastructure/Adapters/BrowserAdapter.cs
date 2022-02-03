using MoneyFox.Core.Interfaces;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace MoneyFox.Win.Infrastructure.Adapters
{
    public class BrowserAdapter : IBrowserAdapter
    {
        public async Task OpenWebsiteAsync(Uri uri)
        {
            Process.Start(uri.ToString());
            await Task.CompletedTask;
        }
    }
}