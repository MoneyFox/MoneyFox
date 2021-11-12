using MoneyFox.Application.Common.Adapters;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace MoneyFox.Desktop.Infrastructure.Adapters
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