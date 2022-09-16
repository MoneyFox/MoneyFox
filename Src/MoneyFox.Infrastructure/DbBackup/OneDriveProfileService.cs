namespace MoneyFox.Infrastructure.DbBackup
{
    using System.IO;
    using System.Threading.Tasks;
    using Flurl.Http;
    using MoneyFox.Core.ApplicationCore.UseCases.DbBackup;
    using MoneyFox.Infrastructure.DbBackup.OneDriveModels;

    internal class OneDriveProfileService : IOneDriveProfileService
    {
        private readonly IOneDriveAuthenticationService oneDriveAuthenticationService;

        public OneDriveProfileService(IOneDriveAuthenticationService oneDriveAuthenticationService)
        {
            this.oneDriveAuthenticationService = oneDriveAuthenticationService;
        }

        public async Task<Stream> GetProfilePictureAsync()
        {
            var authentication = await oneDriveAuthenticationService.AcquireAuthentication();

            var imageStream = await "https://graph.microsoft.com/v1.0/me/photo/$value"
            .WithOAuthBearerToken(authentication.AccessToken)
            .GetStreamAsync();

            return imageStream;
        }

        public async Task<UserAccountDto> GetUserAccountAsync()
        {
            var authentication = await oneDriveAuthenticationService.AcquireAuthentication();
            var userDto = await "https://graph.microsoft.com/v1.0/me"
                .WithOAuthBearerToken(authentication.AccessToken)
                .GetJsonAsync<UserDto>();

            return new UserAccountDto(name: userDto.DisplayName ?? string.Empty, email: userDto.PrincipalName ?? string.Empty);
        }
    }
}
