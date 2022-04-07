namespace MoneyFox.Infrastructure.DbBackup
{

    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;
    using Core._Pending_.DbBackup;
    using Core._Pending_.Exceptions;
    using Microsoft.Graph;
    using Microsoft.Identity.Client;
    using NLog;
    using Logger = NLog.Logger;

    public class OneDriveService : ICloudBackupService
    {
        private const string BACKUP_NAME = "backupmoneyfox3.db";
        private const string BACKUP_NAME_TEMP = "moneyfox.db_upload";
        private const string ARCHIVE_FOLDER_NAME = "Archive";
        private const string BACKUP_ARCHIVE_NAME_TEMPLATE = "backupmoneyfox3_{0}.db";
        private const int BACKUP_ARCHIVE_COUNT = 15;
        private const string ERROR_CODE_CANCELED = "authentication_canceled";
        private readonly string[] scopes = { "Files.ReadWrite", "User.ReadBasic.All" };

        private readonly Logger logManager = LogManager.GetCurrentClassLogger();

        private readonly IPublicClientApplication publicClientApplication;
        private readonly IGraphClientFactory graphClientFactory;

        public OneDriveService(IPublicClientApplication publicClientApplication, IGraphClientFactory graphClientFactory)
        {
            this.publicClientApplication = publicClientApplication;
            this.graphClientFactory = graphClientFactory;
            UserAccount = new UserAccount();
        }

        private GraphServiceClient? GraphServiceClient { get; set; }

        private DriveItem? ArchiveFolder { get; set; }

        public UserAccount UserAccount { get; set; }

        public async Task LoginAsync()
        {
            IEnumerable<IAccount> accounts = await publicClientApplication.GetAccountsAsync();
            try
            {
                var firstAccount = accounts.FirstOrDefault();
                var authResult = firstAccount == null
                    ? await AcquireInteractive()
                    : await publicClientApplication.AcquireTokenSilent(scopes: scopes, account: firstAccount).ExecuteAsync();

                GraphServiceClient = graphClientFactory.CreateClient(authResult);
                var user = await GraphServiceClient.Me.Request().GetAsync();
                UserAccount.SetUserAccount(displayName: user.DisplayName, email: string.IsNullOrEmpty(user.Mail) ? user.UserPrincipalName : user.Mail);
            }
            catch (MsalUiRequiredException ex)
            {
                logManager.Debug(ex);

                throw;
            }
            catch (MsalClientException ex)
            {
                if (ex.ErrorCode == ERROR_CODE_CANCELED)
                {
                    logManager.Info(ex);

                    throw new BackupOperationCanceledException();
                }

                logManager.Error(ex);

                throw;
            }
        }

        public async Task LogoutAsync()
        {
            try
            {
                logManager.Info("Logout.");
                List<IAccount> accounts = (await publicClientApplication.GetAccountsAsync()).ToList();
                while (accounts.Any())
                {
                    await publicClientApplication.RemoveAsync(accounts.FirstOrDefault());
                    accounts = (await publicClientApplication.GetAccountsAsync()).ToList();
                }
            }
            catch (MsalClientException ex)
            {
                if (ex.ErrorCode == ERROR_CODE_CANCELED)
                {
                    logManager.Info(ex);

                    throw new BackupOperationCanceledException();
                }

                logManager.Error(ex);

                throw;
            }
            catch (Exception ex)
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
                if (GraphServiceClient == null)
                {
                    await LoginSilentAsync();
                    if (GraphServiceClient == null)
                    {
                        throw new BackupAuthenticationFailedException("Was not able to automatically login.");
                    }
                }

                var uploadedItem = await GraphServiceClient.Me.Drive.Special.AppRoot.ItemWithPath(BACKUP_NAME_TEMP)
                    .Content.Request()
                    .PutAsync<DriveItem>(dataToUpload);

                await LoadArchiveFolderAsync();
                await DeleteCleanupOldBackupsAsync();
                await ArchiveCurrentBackupAsync();
                await RenameUploadedBackupAsync();

                return uploadedItem != null;
            }
            catch (MsalClientException ex)
            {
                if (ex.ErrorCode == ERROR_CODE_CANCELED)
                {
                    logManager.Info(ex);
                    await RestoreArchivedBackupInCaseOfErrorAsync();

                    throw new BackupOperationCanceledException(ex);
                }

                logManager.Error(ex);
                await RestoreArchivedBackupInCaseOfErrorAsync();

                throw;
            }
            catch (OperationCanceledException ex)
            {
                logManager.Info(ex);
                await RestoreArchivedBackupInCaseOfErrorAsync();

                throw new BackupOperationCanceledException(ex);
            }
            catch (Exception ex)
            {
                logManager.Error(ex);
                await RestoreArchivedBackupInCaseOfErrorAsync();

                throw new BackupAuthenticationFailedException(ex);
            }
        }

        public async Task<Stream> RestoreAsync()
        {
            try
            {
                logManager.Info("Restore Backup.");
                if (GraphServiceClient == null)
                {
                    await LoginSilentAsync();
                    if (GraphServiceClient == null)
                    {
                        throw new BackupAuthenticationFailedException("Was not able to automatically login.");
                    }
                }

                var existingBackup
                    = (await GraphServiceClient.Me.Drive.Special.AppRoot.Children.Request().GetAsync()).FirstOrDefault(x => x.Name == BACKUP_NAME);

                if (existingBackup == null)
                {
                    throw new NoBackupFoundException($"No backup with the name {BACKUP_NAME} was found.");
                }

                return await GraphServiceClient.Drive.Items[existingBackup.Id].Content.Request().GetAsync();
            }
            catch (MsalClientException ex)
            {
                if (ex.ErrorCode == ERROR_CODE_CANCELED)
                {
                    logManager.Info(ex);

                    throw new BackupOperationCanceledException();
                }

                logManager.Error(ex);

                throw;
            }
            catch (Exception ex)
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
                if (GraphServiceClient == null)
                {
                    await LoginSilentAsync();
                    if (GraphServiceClient == null)
                    {
                        return DateTime.MinValue;
                    }
                }

                var existingBackup
                    = (await GraphServiceClient.Me.Drive.Special.AppRoot.Children.Request().GetAsync()).FirstOrDefault(x => x.Name == BACKUP_NAME);

                if (existingBackup != null)
                {
                    return existingBackup.LastModifiedDateTime?.DateTime ?? DateTime.MinValue;
                }

                return DateTime.MinValue;
            }
            catch (Exception ex)
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
                if (GraphServiceClient == null)
                {
                    await LoginSilentAsync();
                    if (GraphServiceClient == null)
                    {
                        throw new BackupAuthenticationFailedException("Was not able to automatically login.");
                    }
                }

                return (await GraphServiceClient.Me.Drive.Special.AppRoot.Children.Request().GetAsync()).Select(x => x.Name).ToList();
            }
            catch (Exception ex)
            {
                logManager.Error(ex);

                throw new BackupAuthenticationFailedException(ex);
            }
        }

        private async Task<AuthenticationResult> AcquireInteractive()
        {
            return await publicClientApplication.AcquireTokenInteractive(scopes)
                .WithUseEmbeddedWebView(true)
                .WithParentActivityOrWindow(ParentActivityWrapper.ParentActivity) // this is required for Android
                .ExecuteAsync();
        }

        /// <summary>
        ///     Login User to OneDrive silently.
        /// </summary>
        private async Task LoginSilentAsync()
        {
            try
            {
                IEnumerable<IAccount> accounts = await publicClientApplication.GetAccountsAsync();
                var firstAccount = accounts.FirstOrDefault();
                var authResult = firstAccount == null
                    ? await publicClientApplication.AcquireTokenInteractive(scopes)
                        .WithUseEmbeddedWebView(true)
                        .WithParentActivityOrWindow(ParentActivityWrapper.ParentActivity) // this is required for Android
                        .ExecuteAsync()
                    : await publicClientApplication.AcquireTokenSilent(scopes: scopes, account: firstAccount).ExecuteAsync();

                GraphServiceClient = graphClientFactory.CreateClient(authResult);
                var user = await GraphServiceClient.Me.Request().GetAsync();
                UserAccount.SetUserAccount(displayName: user.DisplayName, email: string.IsNullOrEmpty(user.Mail) ? user.UserPrincipalName : user.Mail);
            }
            catch (MsalUiRequiredException ex)
            {
                logManager.Info(ex);

                throw new BackupAuthenticationFailedException();
            }
        }

        private async Task RestoreArchivedBackupInCaseOfErrorAsync()
        {
            try
            {
                logManager.Info("Restore archived Backup.");
                if (GraphServiceClient == null)
                {
                    throw new GraphClientNullException();
                }

                var archivedBackups = await GraphServiceClient.Drive.Items[ArchiveFolder?.Id].Children.Request().GetAsync();
                if (!archivedBackups.Any())
                {
                    logManager.Info("No backups found.");

                    return;
                }

                var lastBackup = archivedBackups.OrderByDescending(x => x.CreatedDateTime).First();
                var appRoot = await GraphServiceClient.Me.Drive.Special.AppRoot.Request().GetAsync();
                var updateItem = new DriveItem { ParentReference = new ItemReference { Id = appRoot.Id }, Name = BACKUP_NAME };
                await GraphServiceClient.Drive.Items[lastBackup.Id].Request().UpdateAsync(updateItem);
            }
            catch (Exception ex)
            {
                logManager.Error(exception: ex, "Failed to restore database on fail");
            }
        }

        private async Task DeleteCleanupOldBackupsAsync()
        {
            logManager.Info("Cleanup old backups.");
            if (GraphServiceClient == null)
            {
                throw new GraphClientNullException();
            }

            var archiveBackups = await GraphServiceClient.Drive.Items[ArchiveFolder?.Id].Children.Request().GetAsync();
            if (archiveBackups.Count < BACKUP_ARCHIVE_COUNT)
            {
                return;
            }

            var oldestBackup = archiveBackups.OrderByDescending(x => x.CreatedDateTime).Last();
            await GraphServiceClient.Drive.Items[oldestBackup?.Id].Request().DeleteAsync();
        }

        private async Task ArchiveCurrentBackupAsync()
        {
            logManager.Info("Archive Backup.");
            if (ArchiveFolder == null)
            {
                return;
            }

            if (GraphServiceClient == null)
            {
                throw new GraphClientNullException();
            }

            var currentBackup = (await GraphServiceClient.Me.Drive.Special.AppRoot.Children.Request().GetAsync()).FirstOrDefault(x => x.Name == BACKUP_NAME);
            if (currentBackup == null)
            {
                return;
            }

            var updateItem = new DriveItem
            {
                ParentReference = new ItemReference { Id = ArchiveFolder.Id },
                Name = string.Format(
                    provider: CultureInfo.InvariantCulture,
                    format: BACKUP_ARCHIVE_NAME_TEMPLATE,
                    arg0: DateTime.Now.ToString(format: "yyyy-M-d_hh-mm-ssss", provider: CultureInfo.InvariantCulture))
            };

            await GraphServiceClient.Drive.Items[currentBackup.Id].Request().UpdateAsync(updateItem);
        }

        private async Task RenameUploadedBackupAsync()
        {
            logManager.Info("Rename backup_upload.");
            if (GraphServiceClient == null)
            {
                throw new GraphClientNullException();
            }

            var backup_upload
                = (await GraphServiceClient.Me.Drive.Special.AppRoot.Children.Request().GetAsync()).FirstOrDefault(x => x.Name == BACKUP_NAME_TEMP);

            if (backup_upload == null)
            {
                return;
            }

            var updateItem = new DriveItem { Name = BACKUP_NAME };
            await GraphServiceClient.Drive.Items[backup_upload.Id].Request().UpdateAsync(updateItem);
        }

        private async Task LoadArchiveFolderAsync()
        {
            if (ArchiveFolder != null)
            {
                return;
            }

            if (GraphServiceClient == null)
            {
                throw new GraphClientNullException();
            }

            ArchiveFolder
                = (await GraphServiceClient.Me.Drive.Special.AppRoot.Children.Request().GetAsync()).CurrentPage.FirstOrDefault(
                    x => x.Name == ARCHIVE_FOLDER_NAME);

            if (ArchiveFolder == null)
            {
                await CreateArchiveFolderAsync();
            }
        }

        private async Task CreateArchiveFolderAsync()
        {
            if (GraphServiceClient == null)
            {
                throw new GraphClientNullException();
            }

            var folderToCreate = new DriveItem { Name = ARCHIVE_FOLDER_NAME, Folder = new Folder() };
            ArchiveFolder = await GraphServiceClient.Me.Drive.Special.AppRoot.Children.Request().AddAsync(folderToCreate);
        }
    }

}
