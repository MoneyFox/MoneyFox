namespace MoneyFox.Infrastructure.DbBackup
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.IO;
    using System.Threading.Tasks;
    using Flurl;
    using Flurl.Http;
    using MoneyFox.Core.ApplicationCore.UseCases.DbBackup;
    using MoneyFox.Infrastructure.DbBackup.OneDriveModels;

    internal class OneDriveProfileService : IOneDriveProfileService
    {
        [SuppressMessage("Minor Code Smell", "S1075:URIs should not be hardcoded", Justification = "Can be later moved to configuration")]
        private readonly Uri graphProfileUri = new Uri("https://graph.microsoft.com/v1.0/me");
        private readonly IOneDriveAuthenticationService oneDriveAuthenticationService;

        public OneDriveProfileService(IOneDriveAuthenticationService oneDriveAuthenticationService)
        {
            this.oneDriveAuthenticationService = oneDriveAuthenticationService;
        }

        public async Task<UserAccountDto> GetUserAccountAsync()
        {
            var authentication = await oneDriveAuthenticationService.AcquireAuthentication();
            var userDto = await graphProfileUri
                .WithOAuthBearerToken(authentication.AccessToken)
                .GetJsonAsync<UserDto>();

            return new UserAccountDto(name: userDto.DisplayName ?? string.Empty, email: userDto.PrincipalName ?? string.Empty);
        }

        public async Task<Stream> GetProfilePictureAsync()
        {
            var authentication = await oneDriveAuthenticationService.AcquireAuthentication();

            return await graphProfileUri
                .AppendPathSegments("photo", "$value")
                .WithOAuthBearerToken(authentication.AccessToken)
                .GetStreamAsync();
        }
    }
}
