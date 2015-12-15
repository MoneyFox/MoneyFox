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

        private const string MSA_CLIENT_ID = "https://login.live.com/oauth20_desktop.srf";
        private const string MSA_RETURN_URL = "https://login.live.com/oauth20_desktop.srf";

        private static readonly string[] Scopes = { "onedrive.readwrite", "wl.offline_access", "wl.signin" };

        private readonly IDialogService dialogService;
        private readonly IMvxFileStore fileStore;

        public OneDriveService(IDialogService dialogService, IMvxFileStore fileStore)
        {
            this.dialogService = dialogService;
            this.fileStore = fileStore;
        }

        private IOneDriveClient oneDriveClient { get; set; }

        private Item Root { get; set; }

        private Item BackupFolder { get; set; }

        public bool IsLoggedIn { get; }

        public async Task Login()
        {
            if (oneDriveClient == null)
            {
                oneDriveClient = OneDriveClient.GetMicrosoftAccountClient(
                    MSA_CLIENT_ID,
                    MSA_RETURN_URL,
                    Scopes);

                await oneDriveClient.AuthenticateAsync();
            }

            try
            {
                if (!oneDriveClient.IsAuthenticated)
                {
                    await oneDriveClient.AuthenticateAsync();
                }

                await GetBackupFolder();
            } catch (OneDriveException exception)
            {
                // Swallow authentication cancelled exceptions
                if (!exception.IsMatch(OneDriveErrorCode.AuthenticationCancelled.ToString()))
                {
                    if (exception.IsMatch(OneDriveErrorCode.AuthenticationFailure.ToString()))
                    {
                        await dialogService.ShowMessage(
                            "Authentication failed",
                            "Authentication failed");
                    } else
                    {
                        throw;
                    }
                }
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
            Root = await oneDriveClient.Drive.Root.Request().GetAsync();
            BackupFolder = Root.Children.CurrentPage.FirstOrDefault(x => x.Name == BACKUP_FOLDER_NAME);

            if (BackupFolder == null)
            {
                var folderToCreate = new Item { Name = BACKUP_FOLDER_NAME, Folder = new Folder() };
                BackupFolder = await oneDriveClient.Drive.Items[Root.Id].Children.Request()
                    .AddAsync(folderToCreate);
            }
        }
    }
}