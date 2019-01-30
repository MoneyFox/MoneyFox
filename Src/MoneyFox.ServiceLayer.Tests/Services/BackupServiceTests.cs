using System.Threading.Tasks;
using MoneyFox.BusinessLogic;
using MoneyFox.BusinessLogic.Backup;
using MoneyFox.ServiceLayer.Facades;
using MoneyFox.ServiceLayer.Services;
using Moq;
using Should;
using Xunit;

namespace MoneyFox.ServiceLayer.Tests.Services
{
    public class BackupServiceTests
    {
        [Fact]
        public async Task Login_loginFailed_resultFailed()
        {
            // Arrange
            var backupManagerMock = new Mock<IBackupManager>();
            backupManagerMock.Setup(x => x.Login())
                .ReturnsAsync(OperationResult.Failed(""));

            var settingsFacade = new Mock<ISettingsFacade>();
            settingsFacade.SetupAllProperties();

            var backupService = new BackupService(backupManagerMock.Object, settingsFacade.Object);

            // Act
            var result = await backupService.Login();

            // Assert
            result.Success.ShouldBeFalse();
            settingsFacade.Object.IsBackupAutouploadEnabled.ShouldBeFalse();
            settingsFacade.Object.IsLoggedInToBackupService.ShouldBeFalse();
        }
    }
}
