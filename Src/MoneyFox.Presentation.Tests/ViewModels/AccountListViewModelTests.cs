using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using GenericServices;
using MoneyFox.DataLayer.Entities;
using MoneyFox.Presentation.ViewModels;
using MoneyFox.ServiceLayer.Facades;
using MoneyFox.ServiceLayer.Interfaces;
using MoneyFox.ServiceLayer.Services;
using MoneyFox.ServiceLayer.ViewModels;
using Moq;
using MvvmCross.Logging;
using MvvmCross.Navigation;
using Xunit;

namespace MoneyFox.ServiceLayer.Tests.ViewModels
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
        public void DeleteAccountCommand_AccountNull_DoNothing()
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
                new Mock<IMvxLogProvider>().Object,
                new Mock<IMvxNavigationService>().Object);

            // Act
            viewModel.DeleteAccountCommand.Execute(null);

            // Assert
            crudserServiceMock.Verify(x => x.DeleteAndSaveAsync<Account>(It.IsAny<int>()), Times.Never);
        }

        [Fact]
        public void DeleteAccountCommand_UserReturnFalse_SkipDeletion()
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
                new Mock<IMvxLogProvider>().Object,
                new Mock<IMvxNavigationService>().Object);

            // Act
            viewModel.DeleteAccountCommand.Execute(new AccountViewModel());

            // Assert
            crudserServiceMock.Verify(x => x.DeleteAndSaveAsync<Account>(It.IsAny<int>()), Times.Never);
        }

        [Fact]
        public void DeleteAccountCommand_UserReturnTrue_ExecuteDeletion()
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
                new Mock<IMvxLogProvider>().Object,
                new Mock<IMvxNavigationService>().Object);

            // Act
            viewModel.DeleteAccountCommand.Execute(new AccountViewModel());

            // Assert
            crudserServiceMock.Verify(x => x.DeleteAndSaveAsync<Account>(It.IsAny<int>()), Times.Once);
        }
    }
}