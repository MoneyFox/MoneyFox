using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using GalaSoft.MvvmLight.Views;
using GenericServices;
using MoneyFox.DataLayer.Entities;
using MoneyFox.Domain.Entities;
using MoneyFox.Presentation.Facades;
using MoneyFox.Presentation.Services;
using MoneyFox.Presentation.ViewModels;
using MoneyFox.ServiceLayer.Facades;
using Moq;
using Xunit;
using IDialogService = MoneyFox.Presentation.Interfaces.IDialogService;

namespace MoneyFox.Presentation.Tests.ViewModels
{
    [ExcludeFromCodeCoverage]
    public class AccountListViewModelTests
    {
        public AccountListViewModelTests()
        {
            crudserServiceMock = new Mock<ICrudServicesAsync>();
            crudserServiceMock.SetupAllProperties();
        }

        private readonly Mock<ICrudServicesAsync> crudserServiceMock;

        [Fact]
        public async Task DeleteAccountCommand_AccountNull_DoNothing()
        {
            // Arrange
            crudserServiceMock.Setup(x => x.DeleteAndSaveAsync<Account>(It.IsAny<int>()))
                .Returns(Task.CompletedTask);

            var balanceCalculationService = new Mock<IBalanceCalculationService>();

            var dialogServiceSetup = new Mock<IDialogService>();
            dialogServiceSetup.Setup(x => x.ShowConfirmMessage(It.IsAny<string>(), It.IsAny<string>(), null, null))
                .Returns(Task.FromResult(false));

            var viewModel = new AccountListViewModel(crudserServiceMock.Object,
                balanceCalculationService.Object,
                dialogServiceSetup.Object,
                new Mock<ISettingsFacade>().Object,
                new Mock<INavigationService>().Object);

            // Act
            await viewModel.DeleteAccountCommand.ExecuteAsync(null);

            // Assert
            crudserServiceMock.Verify(x => x.DeleteAndSaveAsync<Account>(It.IsAny<int>()), Times.Never);
        }

        [Fact]
        public async Task DeleteAccountCommand_UserReturnFalse_SkipDeletion()
        {
            // Arrange
            crudserServiceMock.Setup(x => x.DeleteAndSaveAsync<Account>(It.IsAny<int>()))
                .Returns(Task.CompletedTask);

            var balanceCalculationService = new Mock<IBalanceCalculationService>();

            var dialogServiceSetup = new Mock<IDialogService>();
            dialogServiceSetup.Setup(x => x.ShowConfirmMessage(It.IsAny<string>(), It.IsAny<string>(), null, null))
                .Returns(Task.FromResult(false));

            var viewModel = new AccountListViewModel(crudserServiceMock.Object,
                balanceCalculationService.Object,
                dialogServiceSetup.Object,
                new Mock<ISettingsFacade>().Object,
                new Mock<INavigationService>().Object);

            // Act
            await viewModel.DeleteAccountCommand.ExecuteAsync(new AccountViewModel());

            // Assert
            crudserServiceMock.Verify(x => x.DeleteAndSaveAsync<Account>(It.IsAny<int>()), Times.Never);
        }

        [Fact]
        public async Task DeleteAccountCommand_UserReturnTrue_ExecuteDeletion()
        {
            // Arrange
            crudserServiceMock.Setup(x => x.DeleteAndSaveAsync<Account>(It.IsAny<int>()))
                .Returns(Task.CompletedTask);

            var balanceCalculationService = new Mock<IBalanceCalculationService>();

            var dialogServiceSetup = new Mock<IDialogService>();
            dialogServiceSetup.Setup(x => x.ShowConfirmMessage(It.IsAny<string>(), It.IsAny<string>(), null, null))
                .Returns(Task.FromResult(true));

            var viewModel = new AccountListViewModel(crudserServiceMock.Object,
                balanceCalculationService.Object,
                dialogServiceSetup.Object,
                new Mock<ISettingsFacade>().Object,
                new Mock<INavigationService>().Object);

            // Act
            await viewModel.DeleteAccountCommand.ExecuteAsync(new AccountViewModel());

            // Assert
            crudserServiceMock.Verify(x => x.DeleteAndSaveAsync<Account>(It.IsAny<int>()), Times.Once);
        }
    }
}