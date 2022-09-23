namespace MoneyFox.Tests.Infrastructure.DbBackup
{
    using System.Threading.Tasks;
    using Flurl.Http.Testing;
    using MoneyFox.Infrastructure.DbBackup;
    using MoneyFox.Infrastructure.DbBackup.OneDriveModels;
    using NSubstitute;
    using Xunit;

    public class OneDriveProfileServiceShould
    {
        private readonly HttpTest httpTest;
        private readonly OneDriveProfileService oneDriveProfileService;
        private readonly IOneDriveAuthenticationService oneDriveAuthenticationService;

        public OneDriveProfileServiceShould()
        {
            httpTest = new HttpTest();

            oneDriveAuthenticationService = Substitute.For<IOneDriveAuthenticationService>();
            oneDriveProfileService = new OneDriveProfileService(oneDriveAuthenticationService);
        }

        public class GetUserAccountAsync : OneDriveProfileServiceShould
        {

            [Fact]
            public async Task CallCorrectUrl()
            {
                // Assert
                httpTest.RespondWithJson(new UserDto { DisplayName = "Hans Meier", PrincipalName = "hans.meier@test.ch", Email = "hans.meier@test.ch" });
                oneDriveAuthenticationService.AcquireAuthentication().Returns(new OneDriveAuthentication("access-token", "bearer"));

                // Act
                await oneDriveProfileService.GetUserAccountAsync();

                // Assert
                httpTest.ShouldHaveCalled("https://graph.microsoft.com/v1.0/me")
                    .WithOAuthBearerToken();
            }
        }

        public class GetProfilePictureAsync : OneDriveProfileServiceShould
        {

            [Fact]
            public async Task CallCorrectUrl()
            {
                // Assert
                oneDriveAuthenticationService.AcquireAuthentication().Returns(new OneDriveAuthentication("access-token", "bearer"));

                // Act
                await oneDriveProfileService.GetProfilePictureAsync();

                // Assert
                httpTest.ShouldHaveCalled("https://graph.microsoft.com/v1.0/me/photo/$value")
                    .WithOAuthBearerToken();
            }
        }
    }
}
