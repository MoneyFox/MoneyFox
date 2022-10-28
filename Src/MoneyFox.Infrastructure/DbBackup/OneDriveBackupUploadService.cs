namespace MoneyFox.Infrastructure.DbBackup;

using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Core.ApplicationCore.Domain.Exceptions;
using Core.ApplicationCore.UseCases.BackupUpload;
using Flurl;
using Flurl.Http;
using Microsoft.Identity.Client;
using OneDriveModels;

internal class OneDriveBackupUploadService : IBackupUploadService
{
    private const string ERROR_CODE_CANCELED = "authentication_canceled";
    private readonly Uri graphDriveUri = new("https://graph.microsoft.com/v1.0/me/drive");

    private readonly IOneDriveAuthenticationService oneDriveAuthenticationService;

    public OneDriveBackupUploadService(IOneDriveAuthenticationService oneDriveAuthenticationService)
    {
        this.oneDriveAuthenticationService = oneDriveAuthenticationService;
    }

    public async Task<DateTime> GetBackupDateAsync()
    {
        var authentication = await oneDriveAuthenticationService.AcquireAuthentication();
        var appRoot = await graphDriveUri.AppendPathSegments("special", "approot", "children")
            .WithOAuthBearerToken(authentication.AccessToken)
            .GetJsonAsync<FileSearchDto>();

        var existingBackups = appRoot.Files;

        return existingBackups.Any()
            ? existingBackups.OrderByDescending(di => di.LastModifiedDateTime).First().LastModifiedDateTime.DateTime.ToLocalTime()
            : DateTime.MinValue;
    }

    public async Task UploadAsync(string backupName, Stream dataToUpload)
    {
        try
        {
            var authentication = await oneDriveAuthenticationService.AcquireAuthentication();
            StreamContent content = new(dataToUpload);
            _ = await graphDriveUri.AppendPathSegments("special", "approot:", $"{backupName}:", "content")
                .WithOAuthBearerToken(authentication.AccessToken)
                .PutAsync(content);
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
        try
        {
            var authentication = await oneDriveAuthenticationService.AcquireAuthentication();
            var appRoot = await graphDriveUri.AppendPathSegments("special", "approot", "children")
                .WithOAuthBearerToken(authentication.AccessToken)
                .GetJsonAsync<FileSearchDto>();

            return appRoot.Files.Count;
        }
        catch (Exception ex)
        {
            throw new BackupAuthenticationFailedException(ex);
        }
    }

    public async Task DeleteOldest()
    {
        var authentication = await oneDriveAuthenticationService.AcquireAuthentication();
        var appRoot = await graphDriveUri.AppendPathSegments("special", "approot", "children")
            .WithOAuthBearerToken(authentication.AccessToken)
            .GetJsonAsync<FileSearchDto>();

        var existingBackups = appRoot.Files;
        var oldestBackup = existingBackups.OrderByDescending(x => x.CreatedDate).Last();
        _ = await graphDriveUri.AppendPathSegments("items", $"{oldestBackup.Id}").WithOAuthBearerToken(authentication.AccessToken).DeleteAsync();
    }
}
