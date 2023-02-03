namespace MoneyFox.Infrastructure.Tests.DbBackup;

using Flurl.Http.Testing;
using Infrastructure.DbBackup;
using Infrastructure.DbBackup.OneDriveModels;
using NSubstitute;

public class OneDriveProfileServiceTests
{
    private readonly HttpTest httpTest;
    private readonly IOneDriveAuthenticationService oneDriveAuthenticationService;
    private readonly OneDriveProfileService oneDriveProfileService;

    protected OneDriveProfileServiceTests()
    {
        httpTest = new();
        oneDriveAuthenticationService = Substitute.For<IOneDriveAuthenticationService>();
        oneDriveProfileService = new(oneDriveAuthenticationService);
    }

    public class GetUserAccountAsync : OneDriveProfileServiceTests
    {
        [Fact]
        public async Task CallCorrectUrl()
        {
            // Assert
            httpTest.RespondWithJson(new UserDto { DisplayName = "Hans Meier", PrincipalName = "hans.meier@test.ch", Email = "hans.meier@test.ch" });
            oneDriveAuthenticationService.AcquireAuthentication().Returns(new OneDriveAuthentication(accessToken: "access-token", tokenType: "bearer"));

            // Act
            await oneDriveProfileService.GetUserAccountAsync();

            // Assert
            httpTest.ShouldHaveCalled("https://graph.microsoft.com/v1.0/me").WithOAuthBearerToken();
        }
    }

    public class GetProfilePictureAsync : OneDriveProfileServiceTests
    {
        [Fact]
        public async Task CallCorrectUrl()
        {
            // Assert
            oneDriveAuthenticationService.AcquireAuthentication().Returns(new OneDriveAuthentication(accessToken: "access-token", tokenType: "bearer"));

            // Act
            await oneDriveProfileService.GetProfilePictureAsync();

            // Assert
            httpTest.ShouldHaveCalled("https://graph.microsoft.com/v1.0/me/photo/$value").WithOAuthBearerToken();
        }
    }
}
