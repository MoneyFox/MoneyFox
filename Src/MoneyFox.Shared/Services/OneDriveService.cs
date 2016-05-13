using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.OneDrive.Sdk;
using MoneyFox.Shared.Constants;
using MoneyFox.Shared.Extensions;
using MoneyFox.Shared.Interfaces;
using MvvmCross.Platform;
using MvvmCross.Platform.Platform;
using MvvmCross.Plugins.File;

namespace MoneyFox.Shared.Services
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

        /// <summary>
        ///     Shows a login prompt to the user.
        /// </summary>
        public async Task Login()
        {
            if (!IsLoggedIn)
            {
                OneDriveClient = await oneDriveAuthenticator.LoginAsync();
            }
        }

        /// <summary>
        ///     Uploads a copy of the current database.
        /// </summary>
        /// <returns>Returns a TaskCompletionType which indicates if the task was successful or not</returns>
        public async Task<bool> Upload()
        {
            if (OneDriveClient.IsAuthenticated)
            {
                await GetBackupFolder();
            }

            using (var dbstream = fileStore.OpenRead(DatabaseConstants.DB_NAME))
            {
                var uploadedItem = await OneDriveClient
                    .Drive
                    .Root
                    .ItemWithPath(Path.Combine(DatabaseConstants.BACKUP_FOLDER_NAME,
                        DatabaseConstants.BACKUP_NAME))
                    .Content
                    .Request()
                    .PutAsync<Item>(dbstream);

                return uploadedItem != null;
            }
        }

        /// <summary>
        ///     Restores the file with the passed name
        /// </summary
        /// <param name="backupname">Name of the backup to restore</param>
        /// <param name="dbName">filename in which the database shall be restored.</param>
        /// <returns>TaskCompletionType which indicates if the task was successful or not</returns>
        public async Task Restore(string backupname, string dbName)
        {
            if (OneDriveClient.IsAuthenticated)
            {
                await GetBackupFolder();
            }

            var children = await OneDriveClient.Drive.Items[BackupFolder?.Id].Children.Request().GetAsync();
            var existingBackup = children.FirstOrDefault(x => x.Name == backupname);

            if (existingBackup != null)
            {
                var backup = await OneDriveClient.Drive.Items[existingBackup.Id].Content.Request().GetAsync();
                if (fileStore.Exists(dbName))
                {
                    fileStore.DeleteFile(dbName);
                }
                fileStore.WriteFile(dbName, backup.ReadToEnd());
            }
        }

        /// <summary>
        ///     Get's the modification date for the existing backup.
        ///     If there is no backup yet, it will return <see cref="DateTime.MinValue"/>
        /// </summary>
        /// <returns>Date of the last backup.</returns>
        public async Task<DateTime> GetBackupDate()
        {
            if (OneDriveClient.IsAuthenticated)
            {
                await GetBackupFolder();
            }

            try
            {
                var children = await OneDriveClient.Drive.Items[BackupFolder?.Id].Children.Request().GetAsync();
                var existingBackup = children.FirstOrDefault(x => x.Name == DatabaseConstants.BACKUP_NAME);

                if (existingBackup != null)
                {
                    return existingBackup.LastModifiedDateTime?.DateTime ?? DateTime.MinValue;
                }
            }
            catch (Exception ex)
            {
                Mvx.Trace(MvxTraceLevel.Error, ex.Message);
            }

            return DateTime.MinValue;
        }

        private async Task GetBackupFolder()
        {
            var children = await OneDriveClient.Drive.Root.Children.Request().GetAsync();
            BackupFolder =
                children.CurrentPage.FirstOrDefault(x => x.Name == DatabaseConstants.BACKUP_FOLDER_NAME);

            if (BackupFolder == null)
            {
                await CreateBackupFolder();
            }
        }

        private async Task CreateBackupFolder()
        {
            var folderToCreate = new Item
            {
                Name = DatabaseConstants.BACKUP_FOLDER_NAME,
                Folder = new Folder()
            };

            var root = await OneDriveClient.Drive.Root.Request().GetAsync();

            BackupFolder = await OneDriveClient.Drive.Items[root.Id].Children.Request()
                .AddAsync(folderToCreate);
        }

        /// <summary>
        ///     Gets a list with all the filenames who are available in the backup folder.
        ///     The name of the backupfolder is defined in the Constants.
        /// </summary>
        /// <returns>A list with all filenames.</returns>
        public async Task<List<string>> GetFileNames()
        {
            await GetBackupFolder();

            var children = await OneDriveClient.Drive.Items[BackupFolder?.Id].Children.Request().GetAsync();
            return children.Select(x => x.Name).ToList();
        }
    }
}