namespace MoneyFox.Win.Infrastructure.Adapters;

using Core.Interfaces;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

public class BrowserAdapter : IBrowserAdapter
{
    public async Task OpenWebsiteAsync(Uri uri)
    {
        Process.Start(new ProcessStartInfo(uri.ToString()) {UseShellExecute = true});

        await Task.CompletedTask;
    }
}