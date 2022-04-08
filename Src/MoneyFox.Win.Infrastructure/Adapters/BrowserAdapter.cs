namespace MoneyFox.Win.Infrastructure.Adapters;

using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Core.Interfaces;

public class BrowserAdapter : IBrowserAdapter
{
    public async Task OpenWebsiteAsync(Uri uri)
    {
        Process.Start(new ProcessStartInfo(uri.ToString()) { UseShellExecute = true });
        await Task.CompletedTask;
    }
}
