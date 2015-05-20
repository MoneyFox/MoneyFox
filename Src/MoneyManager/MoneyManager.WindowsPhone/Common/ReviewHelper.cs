using MoneyManager.Foundation;
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
        private static string usesBeforeDialogString = "usesBeforeDialog";
        private static string msStoreUrl = "ms-windows-store:reviewapp?appid=";
        private static string reviewQuestion = "DoYouWantToRate";
        private static string positivAnswer = "Yes";
        private static string negativAnswer = "No";
        private static int usesBeforeDialogPopup = 5;

        private static ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;

        public static void AskUserForReview(){
            if (localSettings.Values[usesBeforeDialogString] == null)
            {
                localSettings.Values[usesBeforeDialogString] = 1;
            }
            else
            {
                localSettings.Values[usesBeforeDialogString] = (int)localSettings.Values[usesBeforeDialogString] + 1;
            }
            if ((int)localSettings.Values[usesBeforeDialogString] > usesBeforeDialogPopup) { 
                if (localSettings.Values[isRated] == null || (bool)localSettings.Values[isRated] == false)
                {
                    ShowRateDialogBox();
                    localSettings.Values[isRated] = true;
                }
            }
        }

        private static async void ShowRateDialogBox()
        {
            var loader = new Windows.ApplicationModel.Resources.ResourceLoader();
            var messageDialog = new MessageDialog(Translation.GetTranslation(reviewQuestion));
            messageDialog.Commands.Add(new UICommand(Translation.GetTranslation(positivAnswer), new UICommandInvokedHandler(CommandInvokedHandler)));
            messageDialog.Commands.Add(new UICommand(Translation.GetTranslation(negativAnswer), new UICommandInvokedHandler(CommandInvokedHandler)));
            messageDialog.DefaultCommandIndex = 0;
            messageDialog.CancelCommandIndex = 1;
            await messageDialog.ShowAsync();

        }

        private static async void CommandInvokedHandler(IUICommand command)
        {
            var loader = new Windows.ApplicationModel.Resources.ResourceLoader();
            if (command.Label.Equals(Translation.GetTranslation(positivAnswer)))
            {
                await Windows.System.Launcher.LaunchUriAsync(new Uri(msStoreUrl + CurrentApp.AppId));

            }
        }

    }
}
