using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.OneDrive.Sdk;
using MoneyManager.Core.Extensions;
using MoneyManager.Foundation;
using MoneyManager.Foundation.Interfaces;
using MvvmCross.Plugins.File;
using Xamarin;
using Constants = MoneyManager.Foundation.Constants;

namespace MoneyManager.Core
{
    public class OneDriveService : IBackupService
    {
        private readonly IMvxFileStore fileStore;
        private readonly IOneDriveAuthenticator oneDriveAuthenticator;

        public OneDriveService(IMvxFileStore fileStore, IOneDriveAuthenticator oneDriveAuthenticator)
        {
            this.fileStore = fileStore;
            this.oneDriveAuthenticator = oneDriveAuthenticator;
        }

        private IOneDriveClient OneDriveClient { get; set; }

        private Item BackupFolder { get; set; }

        public bool IsLoggedIn => OneDriveClient?.IsAuthenticated ?? false;

        public async Task Login()
        {
            OneDriveClient = await oneDriveAuthenticator.LoginAsync();

            if (OneDriveClient.IsAuthenticated)
            {
                await GetBackupFolder();
            }
        }

        public async Task<TaskCompletionType> Upload()
        {
            try
            {
                using (var dbstream = fileStore.OpenRead(Constants.DB_NAME))
                {
                    var uploadedItem = await OneDriveClient
                        .Drive
                        .Root
                        .ItemWithPath(Path.Combine(Constants.BACKUP_FOLDER_NAME, Constants.BACKUP_NAME))
                        .Content
                        .Request()
                        .PutAsync<Item>(dbstream);

                    return uploadedItem != null ? TaskCompletionType.Successful : TaskCompletionType.Unsuccessful;
                }
            }
            catch (OneDriveException ex)
            {
                Insights.Report(ex, Insights.Severity.Error);
                return TaskCompletionType.Unsuccessful;
            }
        }

        public async Task<TaskCompletionType> Restore()
        {
            if (BackupFolder == null)
            {
                await Login();
            }

            var children = await OneDriveClient.Drive.Items[BackupFolder?.Id].Children.Request().GetAsync();
            var existingBackup = children.FirstOrDefault(x => x.Name == Constants.BACKUP_NAME);

            if (existingBackup != null)
            {
                var backup = await OneDriveClient.Drive.Items[existingBackup.Id].Content.Request().GetAsync();
                if (fileStore.Exists(Constants.DB_NAME))
                {
                    fileStore.DeleteFile(Constants.DB_NAME);
                }
                fileStore.WriteFile(Constants.DB_NAME, backup.ReadToEnd());
            }

            return TaskCompletionType.Successful;
        }

        private async Task GetBackupFolder()
        {
            var children = await OneDriveClient.Drive.Root.Children.Request().GetAsync();
            BackupFolder = children.CurrentPage.FirstOrDefault(x => x.Name == Constants.BACKUP_FOLDER_NAME);

            if (BackupFolder == null)
            {
                await CreateBackupFolder();
            }
        }

        private async Task CreateBackupFolder()
        {
            var folderToCreate = new Item {Name = Constants.BACKUP_FOLDER_NAME, Folder = new Folder()};

            var root = await OneDriveClient.Drive.Root.Request().GetAsync();

            BackupFolder = await OneDriveClient.Drive.Items[root.Id].Children.Request()
                .AddAsync(folderToCreate);
        }
    }
}