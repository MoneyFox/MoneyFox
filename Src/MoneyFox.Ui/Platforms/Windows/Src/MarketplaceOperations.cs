namespace MoneyFox.Ui.Platforms.Windows.Src;

using global::Windows.ApplicationModel.Store;
using global::Windows.System;
using MoneyFox.Core.Common.Interfaces;

public class MarketplaceOperations : IStoreOperations
{
    private const string STORE_URL = "ms-windows-store:reviewapp?appid=";

    public async void RateApp()
    {
        await Launcher.LaunchUriAsync(new($"{STORE_URL}{CurrentApp.AppId}"));
    }
}
