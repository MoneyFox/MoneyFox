using System;
using System.Threading.Tasks;
using Windows.UI.Popups;
using GalaSoft.MvvmLight;
using Microsoft.Live;
using MoneyManager.Business.Helper;
using MoneyManager.Business.Logic;
using MoneyManager.Foundation;

namespace MoneyManager.Business.ViewModels
{
    public class BackupViewModel: ViewModelBase
    {
        private const string BackupFolderName = "MoneyFoxBackup";
        private const string BackupName = "moneyfoxBackup.sqlite";
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


        public async void CreateBackup()
        {
            var folderId = String.Empty ?? await BackupLogic.CreateBackupFolder(LiveClient, BackupFolderName);

            var backupId = string.Empty ?? await BackupLogic.GetBackupId(LiveClient, folderId, BackupName);
            if (!String.IsNullOrEmpty(backupId))
            {
                await RequestBackupDeletion(backupId);
            }

            await BackupLogic.UploadBackup(LiveClient, folderId);
        }

        private async Task RequestBackupDeletion(string backupId)
        {
            if (await Utilities.IsDeletionConfirmed())
            {
                await LiveClient.DeleteAsync(backupId);
            }
        }
    }
}
