using System;
using Windows.ApplicationModel.Store;
using Windows.System;
using MoneyManager.Foundation.Interfaces;

namespace MoneyManager.Windows.Concrete
{
    public class StoreFeatures : IStoreFeatures
    {
        public async void RateApp()
        {
            await Launcher.LaunchUriAsync(new Uri("ms-windows-store:reviewapp?appid=" + CurrentApp.AppId));
        }
    }
}