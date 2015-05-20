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
        private static string isRated = "rated";
        private static string msStoreUrl = "ms-windows-store:reviewapp?appid=";
        private static string reviewQuestion = "DoYouWantToRate";
        private static string positivAnswer = "Yes";
        private static string negativAnswer = "No";

        ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;

        public void askUserForReview(){
            if (localSettings.Values[isRated] == null || (bool)localSettings.Values[isRated] == false)
            {
                ShowRateDialogBox();
                localSettings.Values[isRated] = true;
            }
        }

        private async void ShowRateDialogBox()
        {
            var loader = new Windows.ApplicationModel.Resources.ResourceLoader();
            var messageDialog = new MessageDialog(loader.GetString(reviewQuestion));
            messageDialog.Commands.Add(new UICommand(loader.GetString(positivAnswer), new UICommandInvokedHandler(this.CommandInvokedHandler)));
            messageDialog.Commands.Add(new UICommand(loader.GetString(negativAnswer), new UICommandInvokedHandler(this.CommandInvokedHandler)));
            messageDialog.DefaultCommandIndex = 0;
            messageDialog.CancelCommandIndex = 1;
            await messageDialog.ShowAsync();

        }

        private async void CommandInvokedHandler(IUICommand command)
        {
            var loader = new Windows.ApplicationModel.Resources.ResourceLoader();
            if (command.Label.Equals(loader.GetString(positivAnswer)))
            {
                await Windows.System.Launcher.LaunchUriAsync(new Uri(msStoreUrl + CurrentApp.AppId));

            }
        }

    }
}
