using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.Graph;
using Microsoft.Identity.Client;
using MoneyFox.Foundation;
using MoneyFox.Foundation.Constants;
using MoneyFox.Foundation.Exceptions;

namespace MoneyFox.BusinessLogic.Backup
{
    /// <inheritdoc />
    public class OneDriveService : ICloudBackupService 
    {
        private readonly string[] scopes = { "User.Read", "Files.ReadWrite"  };
        
        private readonly IPublicClientApplication publicClientApplication;
        
        /// <summary>
        ///     Constructor
        /// </summary>
        public OneDriveService(IPublicClientApplication publicClientApplication)
        {
            this.publicClientApplication = publicClientApplication;
        }

        private IGraphServiceClient GraphServiceClient { get; set; }

        private DriveItem BackupFolder { get; set; }
        private DriveItem ArchiveFolder { get; set; }

        /// <summary>
        ///     Login User to OneDrive.
        /// </summary>
        public async Task Login()
        {
            AuthenticationResult authResult = null;
            IEnumerable<IAccount> accounts = await publicClientApplication.GetAccountsAsync();

            // let's see if we have a user in our belly already
            try
            {
                IAccount firstAccount = accounts.FirstOrDefault();
                authResult = await publicClientApplication.AcquireTokenSilent(scopes, firstAccount).ExecuteAsync();
            } 
            catch (MsalUiRequiredException)
            {
                // pop the browser for the interactive experience
                authResult = await publicClientApplication.AcquireTokenInteractive(scopes)
                                      .WithParentActivityOrWindow(ParentActivityWrapper.ParentActivity) // this is required for Android
                                      .ExecuteAsync();
            }

            GraphServiceClient = new GraphServiceClient(new DelegateAuthenticationProvider((requestMessage) => {
                requestMessage
                    .Headers
                    .Authorization = new AuthenticationHeaderValue("bearer", authResult.AccessToken);

                return Task.FromResult(0);
            }));
        }

        /// <summary>
        ///     Logout User from OneDrive.
        /// </summary>
        public async Task Logout()
        {
            List<IAccount> accounts = (await publicClientApplication.GetAccountsAsync()).ToList();

            while (accounts.Any())
            {
                await publicClientApplication.RemoveAsync(accounts.FirstOrDefault());
                accounts = (await publicClientApplication.GetAccountsAsync()).ToList();
            }
        }

        /// <inheritdoc />
        public async Task<bool> Upload(Stream dataToUpload)
        {
            if (GraphServiceClient == null)
            {
                await Login();
            }

            await LoadBackupFolder();
            await LoadArchiveFolder();

            await DeleteCleanupOldBackups();
            await ArchiveCurrentBackup();

            var uploadedItem = await GraphServiceClient
                .Drive
                .Root
                .ItemWithPath(Path.Combine(DatabaseConstants.BACKUP_FOLDER_NAME,
                                           DatabaseConstants.BACKUP_NAME))
                .Content
                .Request()
                .PutAsync<DriveItem>(dataToUpload);

            return uploadedItem != null;
        }

        /// <inheritdoc />
        public async Task<Stream> Restore(string backupname, string dbName)
        {
            if (GraphServiceClient == null)
            {
                await Login();
            }

            await LoadBackupFolder();

            var children = await GraphServiceClient.Drive
                                               .Items[BackupFolder?.Id]
                                               .Children
                                               .Request()
                                               .GetAsync();
            var existingBackup = children.FirstOrDefault(x => x.Name == backupname);

            if (existingBackup == null)
                throw new NoBackupFoundException($"No backup with the name {backupname} was found.");
            return await GraphServiceClient.Drive.Items[existingBackup.Id].Content.Request().GetAsync();
        }

        /// <inheritdoc />
        public async Task<DateTime> GetBackupDate()
        {
            if (GraphServiceClient == null)
            {
                await Login();
            }

            await LoadBackupFolder();

            var children = await GraphServiceClient.Drive
                                                .Items[BackupFolder?.Id]
                                                .Children
                                                .Request()
                                                .GetAsync();
            var existingBackup = children.FirstOrDefault(x => x.Name == DatabaseConstants.BACKUP_NAME);

            if (existingBackup != null)
            {
                return existingBackup.LastModifiedDateTime?.DateTime ?? DateTime.MinValue;
            }

            return DateTime.MinValue;
        }

        /// <inheritdoc />
        public async Task<List<string>> GetFileNames()
        {
            if (GraphServiceClient == null)
            {
                await Login();
            }

            await LoadBackupFolder();

            var children = await GraphServiceClient.Drive
                                               .Items[BackupFolder?.Id]
                                               .Children
                                               .Request()
                                               .GetAsync();

            return children.Select(x => x.Name).ToList();
        }

        private async Task DeleteCleanupOldBackups()
        {
            var archiveBackups = await GraphServiceClient.Drive
                                                     .Items[ArchiveFolder?.Id]
                                                     .Children
                                                     .Request()
                                                     .GetAsync();

            if (archiveBackups.Count < 5) return;
            var oldestBackup = archiveBackups.OrderByDescending(x => x.CreatedDateTime).Last();

            await GraphServiceClient.Drive
                                .Items[oldestBackup?.Id]
                                .Request()
                                .DeleteAsync();
        }

        private async Task ArchiveCurrentBackup()
        {
            var backups = await GraphServiceClient.Drive
                                              .Items[BackupFolder?.Id]
                                              .Children.Request()
                                              .GetAsync();
            var currentBackup = backups.FirstOrDefault(x => x.Name == DatabaseConstants.BACKUP_NAME);

            if (currentBackup == null) return;

            var updateItem = new DriveItem
            {
                ParentReference = new ItemReference {Id = ArchiveFolder.Id},
                Name = string.Format(CultureInfo.InvariantCulture,
                                     DatabaseConstants.BACKUP_ARCHIVE_NAME,
                                     DateTime.Now.ToString("yyyy-M-d_hh-mm-ssss", CultureInfo.InvariantCulture))
            };

            await GraphServiceClient
                .Drive
                .Items[currentBackup.Id]
                .Request()
                .UpdateAsync(updateItem);
        }

        private async Task LoadBackupFolder()
        {
            if (BackupFolder != null) return;

            var children = await GraphServiceClient.Drive
                                               .Root
                                               .Children
                                               .Request()
                                               .GetAsync();
            BackupFolder =
                children.CurrentPage.FirstOrDefault(x => x.Name == DatabaseConstants.BACKUP_FOLDER_NAME);

            if (BackupFolder == null)
            {
                await CreateBackupFolder();
            }
        }

        private async Task CreateBackupFolder()
        {
            var folderToCreate = new DriveItem
            {
                Name = DatabaseConstants.BACKUP_FOLDER_NAME,
                Folder = new Folder()
            };

            var root = await GraphServiceClient.Drive
                                           .Root
                                           .Request()
                                           .GetAsync();

            BackupFolder = await GraphServiceClient.Drive.Items[root.Id]
                                               .Children.Request()
                                               .AddAsync(folderToCreate);
        }

        private async Task LoadArchiveFolder()
        {
            if (ArchiveFolder != null) return;

            var children = await GraphServiceClient.Drive
                                               .Root
                                               .Children
                                               .Request()
                                               .GetAsync();
            ArchiveFolder = children.CurrentPage.FirstOrDefault(x => x.Name == DatabaseConstants.ARCHIVE_FOLDER_NAME);

            if (ArchiveFolder == null)
            {
                await CreateArchiveFolder();
            }
        }

        private async Task CreateArchiveFolder()
        {
            var folderToCreate = new DriveItem
            {
                Name = DatabaseConstants.ARCHIVE_FOLDER_NAME,
                Folder = new Folder()
            };

            ArchiveFolder = await GraphServiceClient.Drive
                                                .Items[BackupFolder?.Id]
                                                .Children
                                                .Request()
                                                .AddAsync(folderToCreate);
        }
    }
}