namespace MoneyFox.Infrastructure.DbBackup.Legacy;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Domain.Exceptions;
using Flurl;
using Flurl.Http;
using Microsoft.Identity.Client;
using OneDriveModels;

internal class OneDriveService : IOneDriveBackupService
{
    private const string ERROR_CODE_CANCELED = "authentication_canceled";
    private readonly Uri graphDriveUri = new("https://graph.microsoft.com/v1.0/me/drive");

    private readonly IOneDriveAuthenticationService oneDriveAuthenticationService;

    public OneDriveService(IOneDriveAuthenticationService oneDriveAuthenticationService)
    {
        this.oneDriveAuthenticationService = oneDriveAuthenticationService;
    }

    public async Task LoginAsync()
    {
        _ = await oneDriveAuthenticationService.AcquireAuthentication();
    }

    public async Task LogoutAsync()
    {
        await oneDriveAuthenticationService.LogoutAsync();
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

    public async Task<List<string>> GetFileNamesAsync()
    {
        try
        {
            var authentication = await oneDriveAuthenticationService.AcquireAuthentication();
            var appRoot = await graphDriveUri.AppendPathSegments("special", "approot", "children")
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
            var appRoot = await graphDriveUri.AppendPathSegments("special", "approot", "children")
                .WithOAuthBearerToken(authentication.AccessToken)
                .GetJsonAsync<FileSearchDto>();

            if (appRoot.Files.Any() is false)
            {
                throw new NoBackupFoundException();
            }

            var lastBackup = appRoot.Files.OrderByDescending(di => di.LastModifiedDateTime).First();

            return await graphDriveUri.AppendPathSegments("items", $"{lastBackup.Id}", "content")
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
}
