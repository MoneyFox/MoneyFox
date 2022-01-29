using MoneyFox.Core._Pending_.Common.Interfaces;
using System;
using Windows.ApplicationModel.Store;
using Windows.System;

#nullable enable
namespace MoneyFox.Win
{
    public class MarketplaceOperations : IStoreOperations
    {
        private const string STORE_URL = "ms-windows-store:reviewapp?appid=";

        public async void RateApp() => await Launcher.LaunchUriAsync(new Uri($"{STORE_URL}{CurrentApp.AppId}"));
    }
}