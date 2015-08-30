using System;
using Windows.ApplicationModel.Store;
using Windows.System;

namespace MoneyManager.Windows
{
    public class StoreFeatures : IStoreFeatures
    {
        public async void RateApp()
        {
            await Launcher.LaunchUriAsync(new Uri("ms-windows-store:reviewapp?appid=" + CurrentApp.AppId));
        }
    }
}