namespace MoneyFox.Infrastructure.DbBackup.Legacy
{

    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;
    using Core.ApplicationCore.Domain.Exceptions;
    using Flurl;
    using Flurl.Http;
    using Microsoft.Graph;
    using Microsoft.Identity.Client;
    using MoneyFox.Infrastructure.DbBackup.OneDriveModels;

    internal class OneDriveService : IOneDriveBackupService
    {
        private const string BACKUP_NAME_TEMPLATE = "backupmoneyfox3_{0}.db";
        private const int BACKUP_ARCHIVE_COUNT = 15;
        private const string ERROR_CODE_CANCELED = "authentication_canceled";

        [SuppressMessage("Minor Code Smell", "S1075:URIs should not be hardcoded", Justification = "Can be later moved to configuration")]
        private readonly Uri graphDriveUri = new Uri("https://graph.microsoft.com/v1.0/me/drive");

        private readonly IOneDriveAuthenticationService oneDriveAuthenticationService;

        public OneDriveService(IOneDriveAuthenticationService oneDriveAuthenticationService)
        {
            this.oneDriveAuthenticationService = oneDriveAuthenticationService;
        }

        public async Task LoginAsync()
        {
            await oneDriveAuthenticationService.AcquireAuthentication();
        }

        public async Task LogoutAsync()
        {
            await oneDriveAuthenticationService.LogoutAsync();
        }

        public async Task<bool> UploadAsync(Stream dataToUpload)
        {
            var graphServiceClient = await oneDriveAuthenticationService.CreateServiceClient();
            try
            {
                var backupName = string.Format(format: BACKUP_NAME_TEMPLATE, arg0: DateTime.UtcNow.ToString(format: "yyyy-M-d_hh-mm-ssss"));
                var uploadedItem = await graphServiceClient.Me.Drive.Special.AppRoot.ItemWithPath(backupName)
                    .Content.Request()
                    .PutAsync<DriveItem>(dataToUpload);

                await CleanupOldBackupsAsync(graphServiceClient);

                return uploadedItem != null;
            }
            catch (MsalClientException ex)
            {
                if (ex.ErrorCode == ERROR_CODE_CANCELED)
                {
                    throw new BackupOperationCanceledException(ex);
                }

                throw;
            }
            catch (OperationCanceledException ex)
            {
                throw new BackupOperationCanceledException(ex);
            }
            catch (Exception ex)
            {
                throw new BackupAuthenticationFailedException(ex);
            }
        }

        public async Task<DateTime> GetBackupDateAsync()
        {
            var authentication = await oneDriveAuthenticationService.AcquireAuthentication();

            var appRoot = await graphDriveUri
                .AppendPathSegments("special", "approot", "children")
                .WithOAuthBearerToken(authentication.AccessToken)
                .GetJsonAsync<FileSearchDto>();

            var existingBackups = appRoot.Files;
            return existingBackups.Any()
                ? existingBackups.OrderByDescending(di => di.LastModifiedDateTime).First().LastModifiedDateTime.DateTime.ToLocalTime()
                : DateTime.MinValue;
        }

        public async Task<List<string>> GetFileNamesAsync()
        {
            try
            {
                var authentication = await oneDriveAuthenticationService.AcquireAuthentication();

                var appRoot = await graphDriveUri
                    .AppendPathSegments("special", "approot", "children")
                    .WithOAuthBearerToken(authentication.AccessToken)
                    .GetJsonAsync<FileSearchDto>();

                return appRoot.Files.Select(f => f.Name).ToList();
            }
            catch (Exception ex)
            {
                throw new BackupAuthenticationFailedException(ex);
            }
        }

        public async Task<Stream> RestoreAsync()
        {
            try
            {
                var authentication = await oneDriveAuthenticationService.AcquireAuthentication();

                var appRoot = await graphDriveUri
                    .AppendPathSegments("special", "approot", "children")
                    .WithOAuthBearerToken(authentication.AccessToken)
                    .GetJsonAsync<FileSearchDto>();

                if (appRoot.Files.Any() is false)
                {
                    throw new NoBackupFoundException();
                }

                var lastBackup = appRoot.Files.OrderByDescending(di => di.LastModifiedDateTime).First();

                return await graphDriveUri
                    .AppendPathSegments("items", $"{lastBackup.Id}", "content")
                    .WithOAuthBearerToken(authentication.AccessToken)
                    .GetStreamAsync();
            }
            catch (MsalClientException ex)
            {
                if (ex.ErrorCode == ERROR_CODE_CANCELED)
                {
                    throw new BackupOperationCanceledException();
                }

                throw;
            }
            catch (Exception ex)
            {
                throw new BackupAuthenticationFailedException(ex);
            }
        }

        private async Task CleanupOldBackupsAsync(GraphServiceClient graphServiceClient)
        {
            var authentication = await oneDriveAuthenticationService.AcquireAuthentication();
            var appRoot = await graphDriveUri
                .AppendPathSegments("special", "approot", "children")
                .WithOAuthBearerToken(authentication.AccessToken)
                .GetJsonAsync<FileSearchDto>();

            var existingBackups = appRoot.Files;
            if (existingBackups.Count < BACKUP_ARCHIVE_COUNT)
            {
                return;
            }

            var oldestBackup = existingBackups.OrderByDescending(x => x.CreatedDate).Last();
            await graphServiceClient.Drive.Items[oldestBackup?.Id].Request().DeleteAsync();
        }
    }

}
