namespace MoneyFox.Win.Infrastructure.Adapters;

using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using Core.Interfaces;

public class BrowserAdapter : IBrowserAdapter
{
    public async Task OpenWebsiteAsync(Uri uri)
    {
        if (!Regex.IsMatch(uri.ToString(), @"(?<Protocol>\w+):\/\/(?<Domain>[\w@][\w.:@]+)\/?[\w\.?=%&=\-@/$,]*"))
            return;

        Process.Start(new ProcessStartInfo(uri.ToString()) { UseShellExecute = true });
        await Task.CompletedTask;
    }
}
