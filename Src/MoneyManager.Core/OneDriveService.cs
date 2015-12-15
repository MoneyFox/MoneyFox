using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.OneDrive.Sdk;
using MoneyManager.Foundation;
using MoneyManager.Foundation.Interfaces;
using MvvmCross.Plugins.File;

namespace MoneyManager.Core
{
    public class OneDriveService : IBackupService
    {
        public const string BACKUP_FOLDER_NAME = "MoneyFoxBackup";
        public const string DATABASE_NAME = "moneyfox_backup.sqlite";
        public const string BACKUP_NAME = "backupmoneyfox_backup.sqlite";

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

        public Task<TaskCompletionType> Upload()
        {
            throw new NotImplementedException();
        }

        public async Task<TaskCompletionType> Restore()
        {
            if (BackupFolder == null)
            {
                await Login();
            }

            var children = await oneDriveClient.Drive.Items[BackupFolder?.Id].Children.Request().GetAsync();
            var existingBackup = children.FirstOrDefault(x => x.Name == BACKUP_NAME);

            if (existingBackup != null)
            {
                var backup = await oneDriveClient.Drive.Items[existingBackup.Id].Content.Request().GetAsync();
                if (fileStore.Exists(DATABASE_NAME))
                {
                    fileStore.DeleteFile(DATABASE_NAME);
                }

                var buffer = new byte[16 * 1024];
                while (backup.Read(buffer, 0, buffer.Length) > 0)
                {
                    fileStore.WriteFile(DATABASE_NAME, buffer);
                }
            }

            return TaskCompletionType.Successful;
        }

        private async Task GetBackupFolder()
        {
            var children = await oneDriveClient.Drive.Root.Children.Request().GetAsync();
            BackupFolder = children.CurrentPage.FirstOrDefault(x => x.Name == BACKUP_FOLDER_NAME);

            if (BackupFolder == null)
            {
                var folderToCreate = new Item { Name = BACKUP_FOLDER_NAME, Folder = new Folder() };
                BackupFolder = await oneDriveClient.Drive.Items[Root.Id].Children.Request()
                    .AddAsync(folderToCreate);
            }
        }

        private async Task CreateBackupFolder()
        {
            
        }
    }
}