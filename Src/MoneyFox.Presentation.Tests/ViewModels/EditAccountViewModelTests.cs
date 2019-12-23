using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using AutoMapper;
using GalaSoft.MvvmLight.Views;
using MediatR;
using MoneyFox.Application.Accounts.Queries.GetAccountById;
using MoneyFox.Application.Common.CloudBackup;
using MoneyFox.Application.Common.Facades;
using MoneyFox.Domain.Entities;
using MoneyFox.Presentation.ViewModels;
using Moq;
using Xunit;

namespace MoneyFox.Presentation.Tests.ViewModels
{
    [ExcludeFromCodeCoverage]
    public class EditAccountViewModelTests
    {
        private readonly Mock<IMediator> mediatorMock;
        private readonly Mock<IMapper> mapperMock;
        private readonly Mock<ISettingsFacade> settingsFacadeMock;
        private readonly Mock<IBackupService> backupServiceMock;
        private readonly Mock<Application.Common.Interfaces.IDialogService> dialogServiceMock;
        private readonly Mock<INavigationService> navigationServiceMock;

        public EditAccountViewModelTests()
        {
            mediatorMock = new Mock<IMediator>();
            mapperMock = new Mock<IMapper>();
            settingsFacadeMock = new Mock<ISettingsFacade>();
            backupServiceMock = new Mock<IBackupService>();
            dialogServiceMock = new Mock<Application.Common.Interfaces.IDialogService>();
            navigationServiceMock = new Mock<INavigationService>();

            mediatorMock.Setup(x => x.Send(It.IsAny<GetAccountByIdQuery>(), default))
                        .ReturnsAsync(new Account("asdf"));

            mapperMock.Setup(x => x.Map<AccountViewModel>(It.IsAny<Account>()))
                      .Returns(new AccountViewModel());
        }

        [Fact]
        public async Task Initialize_AccountLoaded()
        {
            // Arrange
            const int accountId = 99;

            var editAccountVm = new EditAccountViewModel(mediatorMock.Object,
                                                         mapperMock.Object,
                                                         settingsFacadeMock.Object,
                                                         backupServiceMock.Object,
                                                         dialogServiceMock.Object,
                                                         navigationServiceMock.Object);

            // Act
            editAccountVm.AccountId = accountId;
            await editAccountVm.InitializeCommand.ExecuteAsync();

            // Assert
            mediatorMock.Verify(x => x.Send(It.IsAny<GetAccountByIdQuery>(), default), Times.Once);
        }
    }
}
