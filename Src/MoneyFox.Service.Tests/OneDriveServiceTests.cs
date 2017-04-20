using System.Threading.Tasks;
using Microsoft.OneDrive.Sdk;
using Moq;
using Xunit;
using MoneyFox.Foundation.Interfaces;

namespace MoneyFox.Service.Tests
{
    public class OneDriveServiceTests
    {
        private readonly Mock<IOneDriveClient> mockOneDriveClient;

        public OneDriveServiceTests()
        {
            mockOneDriveClient = new Mock<IOneDriveClient>();
        }

        [Fact]
        public async void Login()
        {
            // Arrange
            var loginCalled = false;
            var authenticator = new Mock<IOneDriveAuthenticator>();
            authenticator.Setup(x => x.LoginAsync()).Callback(() => loginCalled = true).Returns(Task.FromResult(mockOneDriveClient.Object));

            // Act
            await new OneDriveService(authenticator.Object).Login();

            // Assert
            Assert.True(loginCalled);
        }

        [Fact]
        public async void Logout()
        {
            // Arrange
            var logoutCalled = false;
            var authenticator = new Mock<IOneDriveAuthenticator>();
            authenticator.Setup(x => x.LogoutAsync()).Callback(() => logoutCalled = true).Returns(Task.FromResult(""));

            // Act
            await new OneDriveService(authenticator.Object).Logout();

            // Assert
            Assert.True(logoutCalled);
        }
    }
}
