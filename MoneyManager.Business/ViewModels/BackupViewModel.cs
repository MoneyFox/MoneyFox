#region

using System;
using System.Threading.Tasks;
using Windows.UI.Popups;
using BugSense;
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
        private const string BackupName = "backupmoneyfox.sqlite";

        private LiveConnectClient LiveClient { get; set; }

        public bool IsConnected { get; set; }

        public bool IsLoading { get; set; }

        private string creationDate;
        public string CreationDate
        {
            get
            {
                if (LiveClient == null || String.IsNullOrEmpty(creationDate))
                {
                    return Translation.GetTranslation("NeverLabel");
                }

                return creationDate;
            }
            private set { creationDate = value; }
        }

        public async Task LogInToOneDrive()
        {
            LiveClient = await BackupLogic.LogInToOneDrive();

            if (LiveClient == null)
            {
                await ShowNotLoggedInMessage();
            }
            else
            {
                IsConnected = true;
            }
        }

        private static async Task ShowNotLoggedInMessage()
        {
            var dialog = new MessageDialog(Translation.GetTranslation("NotLoggedInMessage"),
                Translation.GetTranslation("NotLoggedIn"));
            dialog.Commands.Add(new UICommand(Translation.GetTranslation("OkLabel")));

            await dialog.ShowAsync();
        }

        public async Task LoadBackupCreationDate()
        {
            if (LiveClient == null) return;
            IsLoading = true;

            var folderId = await BackupLogic.GetFolderId(LiveClient, BackupFolderName);
            if (folderId != null)
            {
                var backupId = await BackupLogic.GetBackupId(LiveClient, folderId, BackupName);
                CreationDate = await BackupLogic.GetBackupCreationDate(LiveClient, backupId);
            }

            IsLoading = false;
        }

        public async Task CreateBackup()
        {
            try
            {
                IsLoading = true;

                await ShowOverwriteInfo();
                var folderId = await BackupLogic.GetFolderId(LiveClient, BackupFolderName);

                if (String.IsNullOrEmpty(folderId))
                {
                    folderId = await BackupLogic.CreateBackupFolder(LiveClient, BackupFolderName);
                }

                var completionType = await BackupLogic.UploadBackup(LiveClient, folderId, DbName);

                await LoadBackupCreationDate();

                await ShowCompletionNote(completionType);
            }
            catch (Exception ex)
            {
                BugSenseHandler.Instance.LogException(ex);
                ShowCompletionNote(TaskCompletionType.Unsuccessful);
            }
            finally
            {
                IsLoading = false;
            }
        }

        private async Task<bool> ShowOverwriteInfo()
        {
            if (!String.IsNullOrEmpty(creationDate))
            {
                var dialog = new MessageDialog(Translation.GetTranslation("OverwriteBackupMessage"),
                    Translation.GetTranslation("OverwriteBackup"));
                dialog.Commands.Add(new UICommand(Translation.GetTranslation("YesLabel")));
                dialog.Commands.Add(new UICommand(Translation.GetTranslation("NoLabel")));

                var result = await dialog.ShowAsync();

                return result.Label == Translation.GetTranslation("YesLabel");
            }
            return true;
        }

        public async Task RestoreBackup()
        {
            try
            {
                IsLoading = true;
                
                var folderId = await BackupLogic.GetFolderId(LiveClient, BackupFolderName);

                await BackupLogic.RestoreBackUp(LiveClient, folderId, "backup" + DbName, DbName);
                await ShowCompletionNote(TaskCompletionType.Successful);
            }
            catch (Exception ex)
            {
                BugSenseHandler.Instance.LogException(ex);
                ShowCompletionNote(TaskCompletionType.Unsuccessful);
            }
            finally
            {
                IsLoading = false;
            }
        }

        private async Task ShowCompletionNote(TaskCompletionType completionType)
        {
            MessageDialog dialog;

            switch (completionType)
            {
                case TaskCompletionType.Successful:
                    dialog = new MessageDialog(Translation.GetTranslation("TaskSuccessfulMessage"), Translation.GetTranslation("SuccessfulTitle"));
                    break;

                case TaskCompletionType.Unsuccessful:
                    dialog = new MessageDialog(Translation.GetTranslation("TaskUnsuccessfulMessage"), Translation.GetTranslation("UnsuccessfulTitle"));
                    break;

                case TaskCompletionType.Aborted:
                    dialog = new MessageDialog(Translation.GetTranslation("TaskAbortedMessage"), Translation.GetTranslation("AbortedTitle"));
                    break;

                default:
                    dialog = new MessageDialog(Translation.GetTranslation("GeneralErrorMessage"), Translation.GetTranslation("GeneralErrorTitle"));
                    break;
            }

            dialog.Commands.Add(new UICommand(Translation.GetTranslation("OkLabel")));

            await dialog.ShowAsync();
        }
    }
}
