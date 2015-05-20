using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Store;
using Windows.Storage;
using Windows.UI.Popups;

namespace MoneyManager.Common
{
    class ReviewHelper
    {

        ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;

        public void rate(){
            if (localSettings.Values["rated"] == null || (bool)localSettings.Values["rated"] == false)
            {
                ShowRateDialogBox();
                localSettings.Values["rated"] = true;
            }
        }

        private async void ShowRateDialogBox()
        {
            var loader = new Windows.ApplicationModel.Resources.ResourceLoader();
            var messageDialog = new MessageDialog(loader.GetString("DoYouWantToRate"));
            messageDialog.Commands.Add(new UICommand(loader.GetString("Yes"), new UICommandInvokedHandler(this.CommandInvokedHandler)));
            messageDialog.Commands.Add(new UICommand(loader.GetString("No"), new UICommandInvokedHandler(this.CommandInvokedHandler)));
            messageDialog.DefaultCommandIndex = 0;
            messageDialog.CancelCommandIndex = 1;
            await messageDialog.ShowAsync();

        }

        private async void CommandInvokedHandler(IUICommand command)
        {
            var loader = new Windows.ApplicationModel.Resources.ResourceLoader();
            if (command.Label.Equals(loader.GetString("Yes")))
            {
                await Windows.System.Launcher.LaunchUriAsync(new Uri("ms-windows-store:reviewapp?appid=" + CurrentApp.AppId));

            }
        }

    }
}
