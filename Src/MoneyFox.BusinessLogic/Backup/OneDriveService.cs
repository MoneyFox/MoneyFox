using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AppCenter.Crashes;
using Microsoft.Graph;
using MoneyFox.Foundation.Constants;
using MoneyFox.Foundation.Exceptions;

namespace MoneyFox.BusinessLogic.Backup
{
    /// <inheritdoc />
    public class OneDriveService : ICloudBackupService
    {
        private readonly IOneDriveAuthenticator oneDriveAuthenticator;

        /// <summary>
        ///     Constructor
        /// </summary>
        public OneDriveService(IOneDriveAuthenticator oneDriveAuthenticator)
        {
            this.oneDriveAuthenticator = oneDriveAuthenticator;
        }

        private IGraphServiceClient OneDriveClient { get; set; }

        private DriveItem BackupFolder { get; set; }
        private DriveItem ArchiveFolder { get; set; }

        /// <summary>
        ///     Login User to OneDrive.
        /// </summary>
        public async Task Login()
        {
            OneDriveClient = await oneDriveAuthenticator.LoginAsync()
                                                        .ConfigureAwait(true);
        }

        /// <summary>
        ///     Logout User from OneDrive.
        /// </summary>
        public async Task Logout()
        {
            await oneDriveAuthenticator.LogoutAsync()
                                       .ConfigureAwait(true);
        }

        /// <inheritdoc />
        public async Task<bool> Upload(Stream dataToUpload)
        {
            if (OneDriveClient == null)
            {
                OneDriveClient = await oneDriveAuthenticator.LoginAsync()
                                                            .ConfigureAwait(true);
            }

            await LoadBackupFolder().ConfigureAwait(true);
            await LoadArchiveFolder().ConfigureAwait(true);

            await DeleteCleanupOldBackups().ConfigureAwait(true);
            await ArchiveCurrentBackup().ConfigureAwait(true);

            var uploadedItem = await OneDriveClient
                .Drive
                .Root
                .ItemWithPath(Path.Combine(DatabaseConstants.BACKUP_FOLDER_NAME,
                                           DatabaseConstants.BACKUP_NAME))
                .Content
                .Request()
                .PutAsync<DriveItem>(dataToUpload)
                .ConfigureAwait(true);

            return uploadedItem != null;
        }

        /// <inheritdoc />
        public async Task<Stream> Restore(string backupname, string dbName)
        {
            if (OneDriveClient == null)
            {
                OneDriveClient = await oneDriveAuthenticator.LoginAsync().ConfigureAwait(true);
            }

            await LoadBackupFolder().ConfigureAwait(true);

            var children = await OneDriveClient.Drive
                                               .Items[BackupFolder?.Id]
                                               .Children
                                               .Request()
                                               .GetAsync()
                                               .ConfigureAwait(true);
            var existingBackup = children.FirstOrDefault(x => x.Name == backupname);

            if (existingBackup == null)
                throw new NoBackupFoundException($"No backup with the name {backupname} was found.");
            return await OneDriveClient.Drive.Items[existingBackup.Id].Content.Request().GetAsync()
                                       .ConfigureAwait(true);
        }

        /// <inheritdoc />
        public async Task<DateTime> GetBackupDate()
        {
            if (OneDriveClient == null)
            {
                OneDriveClient = await oneDriveAuthenticator.LoginAsync().ConfigureAwait(true);
            }

            await LoadBackupFolder().ConfigureAwait(true);

            try
            {
                var children = await OneDriveClient.Drive
                                                   .Items[BackupFolder?.Id]
                                                   .Children
                                                   .Request()
                                                   .GetAsync()
                                                   .ConfigureAwait(true);
                var existingBackup = children.FirstOrDefault(x => x.Name == DatabaseConstants.BACKUP_NAME);

                if (existingBackup != null)
                {
                    return existingBackup.LastModifiedDateTime?.DateTime ?? DateTime.MinValue;
                }
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }

            return DateTime.MinValue;
        }

        /// <inheritdoc />
        public async Task<List<string>> GetFileNames()
        {
            if (OneDriveClient == null)
            {
                OneDriveClient = await oneDriveAuthenticator.LoginAsync()
                                                            .ConfigureAwait(true);
            }

            await LoadBackupFolder().ConfigureAwait(true);

            var children = await OneDriveClient.Drive
                                               .Items[BackupFolder?.Id]
                                               .Children
                                               .Request()
                                               .GetAsync()
                                               .ConfigureAwait(true);

            return children.Select(x => x.Name).ToList();
        }

        private async Task DeleteCleanupOldBackups()
        {
            var archiveBackups = await OneDriveClient.Drive
                                                     .Items[ArchiveFolder?.Id]
                                                     .Children
                                                     .Request()
                                                     .GetAsync()
                                                     .ConfigureAwait(true);

            if (archiveBackups.Count < 5) return;
            var oldestBackup = archiveBackups.OrderByDescending(x => x.CreatedDateTime).Last();

            await OneDriveClient.Drive
                                .Items[oldestBackup?.Id]
                                .Request()
                                .DeleteAsync()
                                .ConfigureAwait(true);
        }

        private async Task ArchiveCurrentBackup()
        {
            var backups = await OneDriveClient.Drive
                                              .Items[BackupFolder?.Id]
                                              .Children.Request()
                                              .GetAsync()
                                              .ConfigureAwait(true);
            var currentBackup = backups.FirstOrDefault(x => x.Name == DatabaseConstants.BACKUP_NAME);

            if (currentBackup == null) return;

            var updateItem = new DriveItem
            {
                ParentReference = new ItemReference {Id = ArchiveFolder.Id},
                Name = string.Format(CultureInfo.InvariantCulture,
                                     DatabaseConstants.BACKUP_ARCHIVE_NAME,
                                     DateTime.Now.ToString("yyyy-M-d_hh-mm-ssss", CultureInfo.InvariantCulture))
            };

            await OneDriveClient
                .Drive
                .Items[currentBackup.Id]
                .Request()
                .UpdateAsync(updateItem)
                .ConfigureAwait(true);
        }

        private async Task LoadBackupFolder()
        {
            if (BackupFolder != null) return;

            var children = await OneDriveClient.Drive
                                               .Root
                                               .Children
                                               .Request()
                                               .GetAsync()
                                               .ConfigureAwait(true);
            BackupFolder =
                children.CurrentPage.FirstOrDefault(x => x.Name == DatabaseConstants.BACKUP_FOLDER_NAME);

            if (BackupFolder == null)
            {
                await CreateBackupFolder().ConfigureAwait(true);
            }
        }

        private async Task CreateBackupFolder()
        {
            var folderToCreate = new DriveItem
            {
                Name = DatabaseConstants.BACKUP_FOLDER_NAME,
                Folder = new Folder()
            };

            var root = await OneDriveClient.Drive
                                           .Root
                                           .Request()
                                           .GetAsync()
                                           .ConfigureAwait(true);

            BackupFolder = await OneDriveClient.Drive.Items[root.Id]
                                               .Children.Request()
                                               .AddAsync(folderToCreate)
                                               .ConfigureAwait(true);
        }

        private async Task LoadArchiveFolder()
        {
            if (ArchiveFolder != null) return;

            var children = await OneDriveClient.Drive
                                               .Root
                                               .Children
                                               .Request()
                                               .GetAsync()
                                               .ConfigureAwait(true);
            ArchiveFolder = children.CurrentPage.FirstOrDefault(x => x.Name == DatabaseConstants.ARCHIVE_FOLDER_NAME);

            if (ArchiveFolder == null)
            {
                await CreateArchiveFolder().ConfigureAwait(true);
            }
        }

        private async Task CreateArchiveFolder()
        {
            var folderToCreate = new DriveItem
            {
                Name = DatabaseConstants.ARCHIVE_FOLDER_NAME,
                Folder = new Folder()
            };

            ArchiveFolder = await OneDriveClient.Drive
                                                .Items[BackupFolder?.Id]
                                                .Children
                                                .Request()
                                                .AddAsync(folderToCreate)
                                                .ConfigureAwait(true);
        }
    }
}