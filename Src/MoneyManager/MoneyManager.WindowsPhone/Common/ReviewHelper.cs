using System;
using Windows.ApplicationModel.Store;
using Windows.Storage;
using Windows.System;
using Windows.UI.Popups;
using MoneyManager.Foundation;

namespace MoneyManager.Common
{
    internal static class ReviewHelper
    {
        private const string IS_RATED = "rated";
        private const string USES_BEFORE_DIALOG_STRING = "usesBeforeDialog";
        private const string MS_STORE_URL = "ms-windows-store:reviewapp?appid=";
        private const string REVIEW_QUESTION = "DoYouWantToRate";
        private const string REVIEW_TITLE = "DoYouWantToRateTitle";
        private const string POSITIV_ANSWER = "Yes";
        private const string NEGATIV_ANSWER = "No";
        private const int USES_BEFORE_DIALOG_POPUP = 5;
        private static readonly ApplicationDataContainer LocalSettings = ApplicationData.Current.LocalSettings;

        public static void AskUserForReview()
        {
            if (LocalSettings.Values[USES_BEFORE_DIALOG_STRING] == null)
            {
                LocalSettings.Values[USES_BEFORE_DIALOG_STRING] = 1;
            }
            else
            {
                LocalSettings.Values[USES_BEFORE_DIALOG_STRING] =
                    (int) LocalSettings.Values[USES_BEFORE_DIALOG_STRING] + 1;
            }
            if ((int) LocalSettings.Values[USES_BEFORE_DIALOG_STRING] > USES_BEFORE_DIALOG_POPUP)
            {
                if (LocalSettings.Values[IS_RATED] == null || (bool) LocalSettings.Values[IS_RATED] == false)
                {
                    ShowRateDialogBox();
                    LocalSettings.Values[IS_RATED] = true;
                }
            }
        }

        private static async void ShowRateDialogBox()
        {
            var messageDialog = new MessageDialog(Translation.GetTranslation(REVIEW_QUESTION),
                Translation.GetTranslation(REVIEW_TITLE));
            messageDialog.Commands.Add(new UICommand(Translation.GetTranslation(POSITIV_ANSWER), CommandInvokedHandler));
            messageDialog.Commands.Add(new UICommand(Translation.GetTranslation(NEGATIV_ANSWER), CommandInvokedHandler));
            messageDialog.DefaultCommandIndex = 0;
            messageDialog.CancelCommandIndex = 1;
            await messageDialog.ShowAsync();
        }

        private static async void CommandInvokedHandler(IUICommand command)
        {
            if (command.Label.Equals(Translation.GetTranslation(POSITIV_ANSWER)))
            {
                await Launcher.LaunchUriAsync(new Uri(MS_STORE_URL + CurrentApp.AppId));
            }
        }
    }
}