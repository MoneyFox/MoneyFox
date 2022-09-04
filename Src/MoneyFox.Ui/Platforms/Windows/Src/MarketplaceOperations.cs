namespace MoneyFox.Win;

using Windows.ApplicationModel.Store;
using Windows.System;
using Core.Common.Interfaces;

public class MarketplaceOperations : IStoreOperations
{
    private const string STORE_URL = "ms-windows-store:reviewapp?appid=";

    public async void RateApp()
    {
        await Launcher.LaunchUriAsync(new($"{STORE_URL}{CurrentApp.AppId}"));
    }
}
