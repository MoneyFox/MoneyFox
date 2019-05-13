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
        private readonly string[] scopes = { "Files.ReadWrite"  };
        
        private readonly IPublicClientApplication publicClientApplication;
        
        /// <summary>
        ///     Constructor
        /// </summary>
        public OneDriveService(IPublicClientApplication publicClientApplication)
        {
            this.publicClientApplication = publicClientApplication;
        }

        private IGraphServiceClient GraphServiceClient { get; set; }

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

            await MoveToAppFolder();
            await LoadArchiveFolder();

            await DeleteCleanupOldBackups();
            await ArchiveCurrentBackup();

            var uploadedItem = await GraphServiceClient
                .Me
                .Drive
                .Special
                .AppRoot
                .ItemWithPath(DatabaseConstants.BACKUP_NAME)
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

            await MoveToAppFolder();
            var existingBackup = (await GraphServiceClient
                    .Me
                    .Drive
                    .Special
                    .AppRoot.Children
                    .Request()
                    .GetAsync())
                .FirstOrDefault(x => x.Name == backupname);

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

            await MoveToAppFolder();

            var existingBackup = (await GraphServiceClient
                    .Me
                    .Drive
                    .Special
                    .AppRoot.Children
                    .Request()
                    .GetAsync())
                .FirstOrDefault(x => x.Name == DatabaseConstants.BACKUP_NAME);

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

            await MoveToAppFolder();

            return (await GraphServiceClient
                    .Me
                    .Drive
                    .Special
                    .AppRoot.Children
                    .Request()
                    .GetAsync())
                .Select(x => x.Name).ToList();
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

        private async Task ArchiveCurrentBackup() {
            var currentBackup = (await GraphServiceClient
                    .Me
                    .Drive
                    .Special
                    .AppRoot.Children
                    .Request()
                    .GetAsync())
                .FirstOrDefault(x => x.Name == DatabaseConstants.BACKUP_NAME);

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

        private async Task MoveToAppFolder()
        {
            var appFolder = await GraphServiceClient
                .Me
                .Drive
                .Special
                .AppRoot
                .Request()
                .GetAsync();

            var children = await GraphServiceClient.Drive
                                               .Root
                                               .Children
                                               .Request()
                                               .GetAsync();
            var backupFolderOld = children.CurrentPage.FirstOrDefault(x => x.Name == DatabaseConstants.BACKUP_FOLDER_NAME);
            if (backupFolderOld != null) {

                var childItems = await GraphServiceClient.Drive
                                        .Items[backupFolderOld.Id]
                                        .Children.Request()
                                        .GetAsync();

                var archive = childItems.FirstOrDefault(x => x.Name == DatabaseConstants.ARCHIVE_FOLDER_NAME);

                var updateItem = new DriveItem
                {
                    ParentReference = new ItemReference { Id = appFolder.Id },
                };

                if (archive != null) {
                    archive.ParentReference = new ItemReference {Id = appFolder.Id};
                    
                    await GraphServiceClient
                        .Drive
                        .Items[archive.Id]
                        .Request()
                        .UpdateAsync(updateItem);
                }

                var currentBackup = childItems.FirstOrDefault(x => x.Name == DatabaseConstants.BACKUP_NAME);
                if (currentBackup != null) {
                    currentBackup.ParentReference = new ItemReference {Id = appFolder.Id};

                    await GraphServiceClient
                        .Drive
                        .Items[currentBackup.Id]
                        .Request()
                        .UpdateAsync(updateItem);
                }

                await GraphServiceClient.Drive
                                        .Items[backupFolderOld.Id]
                                        .Request()
                                        .DeleteAsync();
            }
        }

        private async Task LoadArchiveFolder()
        {
            if (ArchiveFolder != null) return;

            ArchiveFolder = (await GraphServiceClient
                .Me
                .Drive
                .Special
                .AppRoot.Children
                .Request()
                .GetAsync())
                .CurrentPage.FirstOrDefault(x => x.Name == DatabaseConstants.ARCHIVE_FOLDER_NAME);

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

            ArchiveFolder = await GraphServiceClient
                .Me
                .Drive
                .Special
                .AppRoot.Children
                .Request()
                .AddAsync(folderToCreate);
        }
    }
}