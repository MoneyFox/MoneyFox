﻿namespace MoneyFox.Win.Infrastructure.Adapters;

using System;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Core.Interfaces;

public class BrowserAdapter : IBrowserAdapter
{
    public async Task OpenWebsiteAsync(Uri uri)
    {
        if (!Regex.IsMatch(input: uri.ToString(), pattern: @"(?<Protocol>\w+):\/\/(?<Domain>[\w@][\w.:@]+)\/?[\w\.?=%&=\-@/$,]*"))
        {
            return;
        }

        Process.Start(new ProcessStartInfo(uri.ToString()) { UseShellExecute = true });
        await Task.CompletedTask;
    }
}
