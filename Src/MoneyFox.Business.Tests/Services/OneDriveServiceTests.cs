using System.Threading.Tasks;
using Microsoft.OneDrive.Sdk;
using MoneyFox.Business.Services;
using MoneyFox.Foundation.Interfaces;
using Moq;
using MvvmCross.Plugins.File;
using Xunit;

namespace MoneyFox.Business.Tests.Services
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
            await new OneDriveService(new Mock<IMvxFileStore>().Object, authenticator.Object).Login();

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
            await new OneDriveService(new Mock<IMvxFileStore>().Object, authenticator.Object).Logout();

            // Assert
            Assert.True(logoutCalled);
        }
    }
}
