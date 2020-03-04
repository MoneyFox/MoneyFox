using AutoMapper;
using MediatR;
using MoneyFox.Application.Accounts.Commands.DeleteAccountById;
using MoneyFox.Application.Common.Facades;
using MoneyFox.Application.Common.Interfaces;
using MoneyFox.Uwp.Services;
using MoneyFox.Uwp.Src;
using MoneyFox.Uwp.ViewModels;
using Moq;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Xunit;

namespace MoneyFox.Uwp.Tests.ViewModels
{
    [ExcludeFromCodeCoverage]
    public class AccountListViewModelTests
    {
        public AccountListViewModelTests()
        {
            mediatorMock = new Mock<IMediator>();
            mediatorMock.SetupAllProperties();
        }

        private readonly Mock<IMediator> mediatorMock;

        [Fact]
        public async Task DeleteAccountCommand_AccountNull_DoNothing()
        {
            // Arrange
            mediatorMock.Setup(x => x.Send(It.IsAny<DeleteAccountByIdCommand>(), default))
                        .ReturnsAsync(Unit.Value);

            var balanceCalculationService = new Mock<IBalanceCalculationService>();

            var dialogServiceSetup = new Mock<IDialogService>();
            dialogServiceSetup.Setup(x => x.ShowConfirmMessageAsync(It.IsAny<string>(), It.IsAny<string>(), null, null))
                              .Returns(Task.FromResult(false));

            var viewModel = new AccountListViewModel(mediatorMock.Object,
                                                     new Mock<IMapper>().Object,
                                                     balanceCalculationService.Object,
                                                     dialogServiceSetup.Object,
                                                     new Mock<ISettingsFacade>().Object,
                                                     new Mock<NavigationService>().Object);

            // Act
            await viewModel.DeleteAccountCommand.ExecuteAsync(null);

            // Assert
            mediatorMock.Verify(x => x.Send(It.IsAny<DeleteAccountByIdCommand>(), default), Times.Never);
        }

        [Fact]
        public async Task DeleteAccountCommand_UserReturnFalse_SkipDeletion()
        {
            // Arrange
            mediatorMock.Setup(x => x.Send(It.IsAny<DeleteAccountByIdCommand>(), default))
                        .ReturnsAsync(Unit.Value);

            var balanceCalculationService = new Mock<IBalanceCalculationService>();

            var dialogServiceSetup = new Mock<IDialogService>();
            dialogServiceSetup.Setup(x => x.ShowConfirmMessageAsync(It.IsAny<string>(), It.IsAny<string>(), null, null))
                              .Returns(Task.FromResult(false));

            var viewModel = new AccountListViewModel(mediatorMock.Object,
                                                     new Mock<IMapper>().Object,
                                                     balanceCalculationService.Object,
                                                     dialogServiceSetup.Object,
                                                     new Mock<ISettingsFacade>().Object,
                                                     new Mock<NavigationService>().Object);

            // Act
            await viewModel.DeleteAccountCommand.ExecuteAsync(new AccountViewModel());

            // Assert
            mediatorMock.Verify(x => x.Send(It.IsAny<DeleteAccountByIdCommand>(), default), Times.Never);
        }

        [Fact]
        public async Task DeleteAccountCommand_UserReturnTrue_ExecuteDeletion()
        {
            // Arrange
            mediatorMock.Setup(x => x.Send(It.IsAny<DeleteAccountByIdCommand>(), default))
                        .ReturnsAsync(Unit.Value);

            var balanceCalculationService = new Mock<IBalanceCalculationService>();

            var dialogServiceSetup = new Mock<IDialogService>();
            dialogServiceSetup.Setup(x => x.ShowConfirmMessageAsync(It.IsAny<string>(), It.IsAny<string>(), null, null))
                              .Returns(Task.FromResult(true));

            var viewModel = new AccountListViewModel(mediatorMock.Object,
                                                     new Mock<IMapper>().Object,
                                                     balanceCalculationService.Object,
                                                     dialogServiceSetup.Object,
                                                     new Mock<ISettingsFacade>().Object,
                                                     new Mock<NavigationService>().Object);

            // Act
            await viewModel.DeleteAccountCommand.ExecuteAsync(new AccountViewModel());

            // Assert
            mediatorMock.Verify(x => x.Send(It.IsAny<DeleteAccountByIdCommand>(), default), Times.Once);
        }
    }
}
