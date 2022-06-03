namespace MoneyFox.Infrastructure.DbBackup
{

    using System;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.Graph;
    using Microsoft.Identity.Client;
    using MoneyFox.Core.ApplicationCore.Domain.Exceptions;
    using MoneyFox.Core.ApplicationCore.UseCases.BackupUpload;

    internal class OneDriveBackupUploadService : IBackupUploadService
    {
        private const string ERROR_CODE_CANCELED = "authentication_canceled";

        private readonly IOneDriveAuthenticationService oneDriveAuthenticationService;

        public OneDriveBackupUploadService(IOneDriveAuthenticationService oneDriveAuthenticationService)
        {
            this.oneDriveAuthenticationService = oneDriveAuthenticationService;
        }

        public async Task<DateTime> GetBackupDateAsync()
        {
            var graphServiceClient = await oneDriveAuthenticationService.CreateServiceClient();
            var existingBackups = await graphServiceClient.Me.Drive.Special.AppRoot.Children.Request().GetAsync();
            if (existingBackups.Any())
            {
                return existingBackups.OrderByDescending(di => di.LastModifiedDateTime).First().LastModifiedDateTime?.DateTime ?? DateTime.MinValue;
            }

            return DateTime.MinValue;
        }

        public async Task UploadAsync(string backupName, Stream dataToUpload)
        {
            var graphServiceClient = await oneDriveAuthenticationService.CreateServiceClient();
            try
            {
                _ = await graphServiceClient.Me.Drive.Special.AppRoot.ItemWithPath(backupName).Content.Request().PutAsync<DriveItem>(dataToUpload);
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

        public async Task<int> GetBackupCount()
        {
            var graphServiceClient = await oneDriveAuthenticationService.CreateServiceClient();
            var existingBackups = await graphServiceClient.Me.Drive.Special.AppRoot.Children.Request().GetAsync();

            return existingBackups.Count;
        }

        public async Task DeleteOldest()
        {
            var graphServiceClient = await oneDriveAuthenticationService.CreateServiceClient();
            var existingBackups = await graphServiceClient.Me.Drive.Special.AppRoot.Children.Request().GetAsync();
            var oldestBackup = existingBackups.OrderByDescending(x => x.CreatedDateTime).Last();
            await graphServiceClient.Drive.Items[oldestBackup?.Id].Request().DeleteAsync();
        }
    }

}
