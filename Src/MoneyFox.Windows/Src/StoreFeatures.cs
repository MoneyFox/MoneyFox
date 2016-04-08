using System;
using Windows.ApplicationModel.Store;
using Windows.System;
using MoneyFox.Shared.Interfaces;

namespace MoneyFox.Windows
{
    public class StoreFeatures : IStoreFeatures
    {
        private const string STORE_URL = "ms-windows-store:reviewapp?appid=";

        public async void RateApp()
        {
            await Launcher.LaunchUriAsync(new Uri(STORE_URL + CurrentApp.AppId));
        }
    }
}