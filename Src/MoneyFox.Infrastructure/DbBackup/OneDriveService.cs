﻿using Microsoft.Graph;
using Microsoft.Identity.Client;
using MoneyFox.Application.Common;
using MoneyFox.Application.Common.Constants;
using MoneyFox.Application.DbBackup;
using MoneyFox.Domain.Exceptions;
using NLog;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Logger = NLog.Logger;

namespace MoneyFox.Infrastructure.DbBackup
{
    /// <inheritdoc />
    public class OneDriveService : ICloudBackupService
    {
        private const int BACKUP_ARCHIVE_COUNT = 15;
        private const string BACKUP_NAME_TEMP = "moneyfox.db_upload";
        private const string ERROR_CODE_CANCELED = "authentication_canceled";
        private readonly string[] scopes = {"Files.ReadWrite", "User.ReadBasic.All"};

        private readonly Logger logManager = LogManager.GetCurrentClassLogger();

        private readonly IPublicClientApplication publicClientApplication;
        private readonly IGraphClientFactory graphClientFactory;

        /// <summary>
        ///     Constructor
        /// </summary>
        public OneDriveService(IPublicClientApplication publicClientApplication, IGraphClientFactory graphClientFactory)
        {
            this.publicClientApplication = publicClientApplication;
            this.graphClientFactory = graphClientFactory;

            UserAccount = new UserAccount();
        }

        private GraphServiceClient? GraphServiceClient { get; set; }

        private DriveItem? ArchiveFolder { get; set; }

        public UserAccount UserAccount { get; set; }

        /// <summary>
        ///     Login User to OneDrive.
        /// </summary>
        public async Task LoginAsync()
        {
            IEnumerable<IAccount> accounts = await publicClientApplication.GetAccountsAsync();

            // let's see if we have a user in our belly already
            try
            {
                IAccount firstAccount = accounts.FirstOrDefault();
                AuthenticationResult authResult = firstAccount == null
                    ? await publicClientApplication.AcquireTokenInteractive(scopes)
                                                   .WithParentActivityOrWindow(
                                                       ParentActivityWrapper
                                                           .ParentActivity) // this is required for Android
                                                   .ExecuteAsync()
                    : await publicClientApplication.AcquireTokenSilent(scopes, firstAccount).ExecuteAsync();

                GraphServiceClient = graphClientFactory.CreateClient(authResult);
                User user = await GraphServiceClient.Me.Request().GetAsync();
                UserAccount.SetUserAccount(
                    user.DisplayName,
                    string.IsNullOrEmpty(user.Mail) ? user.UserPrincipalName : user.Mail);
            }
            catch(MsalUiRequiredException ex)
            {
                logManager.Debug(ex);
                throw;
            }
            catch(MsalClientException ex)
            {
                if(ex.ErrorCode == ERROR_CODE_CANCELED)
                {
                    logManager.Info(ex);
                    throw new BackupOperationCanceledException();
                }

                logManager.Error(ex);
                throw;
            }
        }

        /// <summary>
        ///     Login User to OneDrive silently.
        /// </summary>
        private async Task LoginSilentAsync()
        {
            try
            {
                IEnumerable<IAccount> accounts = await publicClientApplication.GetAccountsAsync();
                IAccount? firstAccount = accounts.FirstOrDefault();

                if(firstAccount == null)
                {
                    throw new BackupAuthenticationFailedException();
                }

                AuthenticationResult authResult =
                    await publicClientApplication.AcquireTokenSilent(scopes, firstAccount).ExecuteAsync();

                GraphServiceClient = graphClientFactory.CreateClient(authResult);
                User user = await GraphServiceClient.Me.Request().GetAsync();
                UserAccount.SetUserAccount(
                    user.DisplayName,
                    string.IsNullOrEmpty(user.Mail) ? user.UserPrincipalName : user.Mail);
            }
            catch(MsalUiRequiredException ex)
            {
                logManager.Info(ex);
                throw new BackupAuthenticationFailedException();
            }
        }

        /// <summary>
        ///     Logout User from OneDrive.
        /// </summary>
        public async Task LogoutAsync()
        {
            try
            {
                logManager.Info("Logout.");

                List<IAccount> accounts = (await publicClientApplication.GetAccountsAsync()).ToList();

                while(accounts.Any())
                {
                    await publicClientApplication.RemoveAsync(accounts.FirstOrDefault());
                    accounts = (await publicClientApplication.GetAccountsAsync()).ToList();
                }
            }
            catch(MsalClientException ex)
            {
                if(ex.ErrorCode == ERROR_CODE_CANCELED)
                {
                    logManager.Info(ex);
                    throw new BackupOperationCanceledException();
                }

                logManager.Error(ex);
                throw;
            }
            catch(Exception ex)
            {
                logManager.Error(ex);
                throw new BackupAuthenticationFailedException(ex);
            }
        }

        /// <inheritdoc />
        public async Task<bool> UploadAsync(Stream dataToUpload)
        {
            try
            {
                logManager.Info("Upload Backup.");

                if(GraphServiceClient == null)
                {
                    await LoginSilentAsync();
                    if(GraphServiceClient == null)
                    {
                        throw new BackupAuthenticationFailedException("Was not able to automatically login.");
                    }
                }

                var uploadedItem = await GraphServiceClient
                                         .Me
                                         .Drive
                                         .Special
                                         .AppRoot
                                         .ItemWithPath(BACKUP_NAME_TEMP)
                                         .Content
                                         .Request()
                                         .PutAsync<DriveItem>(dataToUpload);

                await LoadArchiveFolderAsync();
                await DeleteCleanupOldBackupsAsync();
                await ArchiveCurrentBackupAsync();
                await RenameUploadedBackupAsync();

                return uploadedItem != null;
            }
            catch(MsalClientException ex)
            {
                if(ex.ErrorCode == ERROR_CODE_CANCELED)
                {
                    logManager.Info(ex);
                    await RestoreArchivedBackupInCaseOfErrorAsync();
                    throw new BackupOperationCanceledException(ex);
                }

                logManager.Error(ex);
                await RestoreArchivedBackupInCaseOfErrorAsync();
                throw;
            }
            catch(OperationCanceledException ex)
            {
                logManager.Info(ex);
                await RestoreArchivedBackupInCaseOfErrorAsync();
                throw new BackupOperationCanceledException(ex);
            }
            catch(Exception ex)
            {
                logManager.Error(ex);
                await RestoreArchivedBackupInCaseOfErrorAsync();
                throw new BackupAuthenticationFailedException(ex);
            }
        }

        private async Task RestoreArchivedBackupInCaseOfErrorAsync()
        {
            logManager.Info("Restore archived Backup.");

            if(GraphServiceClient == null)
            {
                throw new GraphClientNullException();
            }

            IDriveItemChildrenCollectionPage archivedBackups = await GraphServiceClient.Drive
                .Items[ArchiveFolder?.Id]
                .Children
                .Request()
                .GetAsync();

            if(!archivedBackups.Any())
            {
                logManager.Info("No backups found.");
                return;
            }

            DriveItem lastBackup = archivedBackups.OrderByDescending(x => x.CreatedDateTime).First();

            DriveItem? appRoot = await GraphServiceClient
                                       .Me
                                       .Drive
                                       .Special
                                       .AppRoot
                                       .Request()
                                       .GetAsync();

            var updateItem = new DriveItem
            {
                ParentReference = new ItemReference {Id = appRoot.Id}, Name = DatabaseConstants.BACKUP_NAME
            };

            await GraphServiceClient
                  .Drive
                  .Items[lastBackup.Id]
                  .Request()
                  .UpdateAsync(updateItem);
        }

        /// <inheritdoc />
        public async Task<Stream> RestoreAsync(string backupName, string dbName)
        {
            try
            {
                logManager.Info("Restore Backup.");

                if(GraphServiceClient == null)
                {
                    await LoginSilentAsync();
                    if(GraphServiceClient == null)
                    {
                        throw new BackupAuthenticationFailedException("Was not able to automatically login.");
                    }
                }

                DriveItem existingBackup = (await GraphServiceClient
                                                  .Me
                                                  .Drive
                                                  .Special
                                                  .AppRoot
                                                  .Children
                                                  .Request()
                                                  .GetAsync())
                    .FirstOrDefault(x => x.Name == backupName);

                if(existingBackup == null)
                {
                    throw new NoBackupFoundException($"No backup with the name {backupName} was found.");
                }

                return await GraphServiceClient.Drive.Items[existingBackup.Id].Content.Request().GetAsync();
            }
            catch(MsalClientException ex)
            {
                if(ex.ErrorCode == ERROR_CODE_CANCELED)
                {
                    logManager.Info(ex);
                    throw new BackupOperationCanceledException();
                }

                logManager.Error(ex);
                throw;
            }
            catch(Exception ex)
            {
                logManager.Error(ex);
                throw new BackupAuthenticationFailedException(ex);
            }
        }

        /// <inheritdoc />
        public async Task<DateTime> GetBackupDateAsync()
        {
            try
            {
                logManager.Info("Get Backupdate.");

                if(GraphServiceClient == null)
                {
                    await LoginSilentAsync();
                    if(GraphServiceClient == null)
                    {
                        return DateTime.MinValue;
                    }
                }

                DriveItem existingBackup = (await GraphServiceClient
                                                  .Me
                                                  .Drive
                                                  .Special
                                                  .AppRoot
                                                  .Children
                                                  .Request()
                                                  .GetAsync())
                    .FirstOrDefault(x => x.Name == DatabaseConstants.BACKUP_NAME);

                if(existingBackup != null)
                {
                    return existingBackup.LastModifiedDateTime?.DateTime ?? DateTime.MinValue;
                }

                return DateTime.MinValue;
            }
            catch(Exception ex)
            {
                logManager.Error(ex);
                throw;
            }
        }

        /// <inheritdoc />
        public async Task<List<string>> GetFileNamesAsync()
        {
            try
            {
                logManager.Info("Get Filenames.");

                if(GraphServiceClient == null)
                {
                    await LoginSilentAsync();
                    if(GraphServiceClient == null)
                    {
                        throw new BackupAuthenticationFailedException("Was not able to automatically login.");
                    }
                }

                return (await GraphServiceClient
                              .Me
                              .Drive
                              .Special
                              .AppRoot
                              .Children
                              .Request()
                              .GetAsync())
                       .Select(x => x.Name)
                       .ToList();
            }
            catch(Exception ex)
            {
                logManager.Error(ex);
                throw new BackupAuthenticationFailedException(ex);
            }
        }

        private async Task DeleteCleanupOldBackupsAsync()
        {
            logManager.Info("Cleanup old backups.");

            if(GraphServiceClient == null)
            {
                throw new GraphClientNullException();
            }

            IDriveItemChildrenCollectionPage archiveBackups = await GraphServiceClient.Drive
                .Items[ArchiveFolder?.Id]
                .Children
                .Request()
                .GetAsync();

            if(archiveBackups.Count < BACKUP_ARCHIVE_COUNT)
            {
                return;
            }

            DriveItem oldestBackup = archiveBackups.OrderByDescending(x => x.CreatedDateTime).Last();

            await GraphServiceClient.Drive
                                    .Items[oldestBackup?.Id]
                                    .Request()
                                    .DeleteAsync();
        }

        private async Task ArchiveCurrentBackupAsync()
        {
            logManager.Info("Archive Backup.");

            if(ArchiveFolder == null)
            {
                return;
            }

            if(GraphServiceClient == null)
            {
                throw new GraphClientNullException();
            }

            DriveItem currentBackup = (await GraphServiceClient
                                             .Me
                                             .Drive
                                             .Special
                                             .AppRoot.Children
                                             .Request()
                                             .GetAsync())
                .FirstOrDefault(x => x.Name == DatabaseConstants.BACKUP_NAME);

            if(currentBackup == null)
            {
                return;
            }

            var updateItem = new DriveItem
            {
                ParentReference = new ItemReference {Id = ArchiveFolder.Id},
                Name = string.Format(
                    CultureInfo.InvariantCulture,
                    DatabaseConstants.BACKUP_ARCHIVE_NAME,
                    DateTime.Now.ToString("yyyy-M-d_hh-mm-ssss", CultureInfo.InvariantCulture))
            };

            await GraphServiceClient
                  .Drive
                  .Items[currentBackup.Id]
                  .Request()
                  .UpdateAsync(updateItem);
        }

        private async Task RenameUploadedBackupAsync()
        {
            logManager.Info("Rename backup_upload.");

            if(GraphServiceClient == null)
            {
                throw new GraphClientNullException();
            }

            DriveItem backup_upload = (await GraphServiceClient
                                             .Me
                                             .Drive
                                             .Special
                                             .AppRoot.Children
                                             .Request()
                                             .GetAsync())
                .FirstOrDefault(x => x.Name == BACKUP_NAME_TEMP);

            if(backup_upload == null)
            {
                return;
            }

            var updateItem = new DriveItem {Name = DatabaseConstants.BACKUP_NAME};

            await GraphServiceClient
                  .Drive
                  .Items[backup_upload.Id]
                  .Request()
                  .UpdateAsync(updateItem);
        }

        private async Task LoadArchiveFolderAsync()
        {
            if(ArchiveFolder != null)
            {
                return;
            }

            if(GraphServiceClient == null)
            {
                throw new GraphClientNullException();
            }

            ArchiveFolder = (await GraphServiceClient
                                   .Me
                                   .Drive
                                   .Special
                                   .AppRoot.Children
                                   .Request()
                                   .GetAsync())
                            .CurrentPage
                            .FirstOrDefault(x => x.Name == DatabaseConstants.ARCHIVE_FOLDER_NAME);

            if(ArchiveFolder == null)
            {
                await CreateArchiveFolderAsync();
            }
        }

        private async Task CreateArchiveFolderAsync()
        {
            if(GraphServiceClient == null)
            {
                throw new GraphClientNullException();
            }

            var folderToCreate = new DriveItem {Name = DatabaseConstants.ARCHIVE_FOLDER_NAME, Folder = new Folder()};

            ArchiveFolder = await GraphServiceClient
                                  .Me
                                  .Drive
                                  .Special
                                  .AppRoot
                                  .Children
                                  .Request()
                                  .AddAsync(folderToCreate);
        }
    }
}