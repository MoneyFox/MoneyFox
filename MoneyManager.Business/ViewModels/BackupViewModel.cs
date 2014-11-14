#region

using System;
using System.Threading.Tasks;
using Windows.UI.Popups;
using GalaSoft.MvvmLight;
using Microsoft.Live;
using MoneyManager.Business.Helper;
using MoneyManager.Business.Logic;
using MoneyManager.Foundation;

#endregion

namespace MoneyManager.Business.ViewModels
{
    public class BackupViewModel : ViewModelBase
    {
        private const string BackupFolderName = "MoneyFoxBackup";
        private const string DbName = "moneyfox.sqlite";

        private LiveConnectClient LiveClient { get; set; }

        public async void LogInToOneDrive()
        {
            LiveClient = await BackupLogic.LogInToOneDrive();

            if (LiveClient == null)
            {
                await ShowNotLoggedInMessage();
            }
        }

        private static async Task ShowNotLoggedInMessage()
        {
            var dialog = new MessageDialog(Translation.GetTranslation("NotLoggedInMessage"),
                Translation.GetTranslation("NotLoggedIn"));
            dialog.Commands.Add(new UICommand(Translation.GetTranslation("OkLabel")));

            await dialog.ShowAsync();
        }
        
        public async Task CreateBackup()
        {
            var folderId = await BackupLogic.GetFolderId(LiveClient, BackupFolderName);

            if (String.IsNullOrEmpty(folderId))
            {
                folderId = await BackupLogic.CreateBackupFolder(LiveClient, BackupFolderName);
            }

            await ShowOverwriteInfo();

            var completionType = await BackupLogic.UploadBackup(LiveClient, folderId, DbName);
            await ShowCompletionNote(completionType);
        }

        private async Task<bool> ShowOverwriteInfo()
        {
            var dialog = new MessageDialog(Translation.GetTranslation("OverwriteBackupMessage"),
                Translation.GetTranslation("OverwriteBackup"));
            dialog.Commands.Add(new UICommand(Translation.GetTranslation("YesLabel")));
            dialog.Commands.Add(new UICommand(Translation.GetTranslation("NoLabel")));

            var result = await dialog.ShowAsync();

            return result.Label == Translation.GetTranslation("YesLabel");
        }

        private async Task ShowCompletionNote(TaskCompletionType completionType)
        {
            MessageDialog dialog;

            switch (completionType)
            {
                case TaskCompletionType.Successful:
                    dialog = new MessageDialog("BackupSuccessfulMessage", "SuccessfulTitle");
                    break;

                case TaskCompletionType.Unsuccessful:
                    dialog = new MessageDialog("BackupUnsuccessfulMessage", "UnsuccessfulTitle");
                    break;

                case TaskCompletionType.Aborted:
                    dialog = new MessageDialog("BackupAbortedMessage", "AbortedTitle");
                    break;

                default:
                    dialog = new MessageDialog("GeneralErrorMessage", "GeneralErrorTitle");
                    break;
            }

            dialog.Commands.Add(new UICommand(Translation.GetTranslation("OkLabel")));
            await dialog.ShowAsync();
        }
    }
}
