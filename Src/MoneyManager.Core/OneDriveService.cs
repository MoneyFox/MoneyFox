using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.OneDrive.Sdk;
using MoneyManager.Core.Helpers;
using MoneyManager.Foundation;
using MoneyManager.Foundation.Interfaces;
using MvvmCross.Plugins.File;

namespace MoneyManager.Core
{
    public class OneDriveService : IBackupService
    {
        private readonly IDialogService dialogService;
        private readonly IMvxFileStore fileStore;
        private readonly IOneDriveAuthenticator oneDriveAuthenticator;

        public OneDriveService(IDialogService dialogService, IMvxFileStore fileStore, IOneDriveAuthenticator oneDriveAuthenticator)
        {
            this.dialogService = dialogService;
            this.fileStore = fileStore;
            this.oneDriveAuthenticator = oneDriveAuthenticator;
        }

        private IOneDriveClient oneDriveClient { get; set; }

        private Item Root { get; set; }

        private Item BackupFolder { get; set; }

        public bool IsLoggedIn { get; }

        public async Task Login()
        {
            oneDriveClient = await oneDriveAuthenticator.LoginAsync();

            if (oneDriveClient.IsAuthenticated)
            {
                await GetBackupFolder();
            }
        }

        public async Task<TaskCompletionType> Upload()
        {
            try
            {
                using (var dbstream = fileStore.OpenRead(Foundation.Constants.DB_NAME))
                {
                    var uploadedItem = await oneDriveClient
                        .Drive
                        .Root
                        .ItemWithPath(Path.Combine(Foundation.Constants.BACKUP_FOLDER_NAME,
                            Foundation.Constants.BACKUP_NAME))
                        .Content
                        .Request()
                        .PutAsync<Item>(dbstream);

                    return uploadedItem != null ? TaskCompletionType.Successful : TaskCompletionType.Unsuccessful;
                }
            }
            catch (OneDriveException ex)
            {
                InsightHelper.Report(ex);
                return TaskCompletionType.Unsuccessful;
            }
        }

        public async Task<TaskCompletionType> Restore()
        {
            if (BackupFolder == null)
            {
                await Login();
            }

            var children = await oneDriveClient.Drive.Items[BackupFolder?.Id].Children.Request().GetAsync();
            var existingBackup = children.FirstOrDefault(x => x.Name == Foundation.Constants.BACKUP_NAME);

            if (existingBackup != null)
            {
                var backup = await oneDriveClient.Drive.Items[existingBackup.Id].Content.Request().GetAsync();
                if (fileStore.Exists(Foundation.Constants.DB_NAME))
                {
                    fileStore.DeleteFile(Foundation.Constants.DB_NAME);
                }
                fileStore.WriteFile(Foundation.Constants.DB_NAME, backup.ReadToEnd());
            }

            return TaskCompletionType.Successful;
        }

        private async Task GetBackupFolder()
        {
            var children = await oneDriveClient.Drive.Root.Children.Request().GetAsync();
            BackupFolder = children.CurrentPage.FirstOrDefault(x => x.Name == Foundation.Constants.BACKUP_FOLDER_NAME);

            if (BackupFolder == null)
            {
                var folderToCreate = new Item { Name = Foundation.Constants.BACKUP_FOLDER_NAME, Folder = new Folder() };
                BackupFolder = await oneDriveClient.Drive.Items[Root.Id].Children.Request()
                    .AddAsync(folderToCreate);
            }
        }
    }
}