namespace MoneyFox.Infrastructure.DbBackup;

using System;
using System.IO;
using System.Threading.Tasks;
using Core.Features.DbBackup;
using Flurl;
using Flurl.Http;
using OneDriveModels;

internal class OneDriveProfileService : IOneDriveProfileService
{
    private readonly Uri graphProfileUri = new("https://graph.microsoft.com/v1.0/me");
    private readonly IOneDriveAuthenticationService oneDriveAuthenticationService;

    public OneDriveProfileService(IOneDriveAuthenticationService oneDriveAuthenticationService)
    {
        this.oneDriveAuthenticationService = oneDriveAuthenticationService;
    }

    public async Task<UserAccountDto> GetUserAccountAsync()
    {
        var authentication = await oneDriveAuthenticationService.AcquireAuthentication();
        var userDto = await graphProfileUri.WithOAuthBearerToken(authentication.AccessToken).GetJsonAsync<UserDto>();

        return new(name: userDto.DisplayName ?? string.Empty, email: userDto.PrincipalName ?? string.Empty);
    }

    public async Task<Stream> GetProfilePictureAsync()
    {
        var authentication = await oneDriveAuthenticationService.AcquireAuthentication();

        return await graphProfileUri.AppendPathSegments("photo", "$value").WithOAuthBearerToken(authentication.AccessToken).GetStreamAsync();
    }
}
