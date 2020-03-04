using AutoMapper;
using MediatR;
using MoneyFox.Application.Accounts.Queries.GetAccountById;
using MoneyFox.Application.Common.Interfaces;
using MoneyFox.Domain.Entities;
using MoneyFox.Uwp.Services;
using MoneyFox.Uwp.ViewModels;
using Moq;
using Should;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using Windows.Globalization;
using Xunit;

namespace MoneyFox.Presentation.Tests.ViewModels
{
    [ExcludeFromCodeCoverage]
    public class EditAccountViewModelTests
    {
        private readonly Mock<IMediator> mediatorMock;
        private readonly Mock<IMapper> mapperMock;
        private readonly Mock<IDialogService> dialogServiceMock;
        private readonly Mock<INavigationService> navigationServiceMock;

        public EditAccountViewModelTests()
        {
            mediatorMock = new Mock<IMediator>();
            mapperMock = new Mock<IMapper>();
            dialogServiceMock = new Mock<IDialogService>();
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
                                                         dialogServiceMock.Object,
                                                         navigationServiceMock.Object);

            // Act
            editAccountVm.AccountId = accountId;
            await editAccountVm.InitializeCommand.ExecuteAsync();

            // Assert
            mediatorMock.Verify(x => x.Send(It.IsAny<GetAccountByIdQuery>(), default), Times.Once);
        }

        [Fact]
        public async Task AmountStringSetOnInitialize()
        {
            // Arrange            
            var cultureString = "en-Gb";
            CultureInfo ci = new CultureInfo(cultureString);
            Thread.CurrentThread.CurrentCulture = ci;
            Thread.CurrentThread.CurrentUICulture = ci;
            ApplicationLanguages.PrimaryLanguageOverride = cultureString;
            const int accountId = 99;

            mapperMock.Setup(x => x.Map<AccountViewModel>(It.IsAny<Account>()))
                      .Returns(new AccountViewModel { CurrentBalance = 99 });

            var editAccountVm = new EditAccountViewModel(mediatorMock.Object,
                                                         mapperMock.Object,
                                                         dialogServiceMock.Object,
                                                         navigationServiceMock.Object);

            // Act
            editAccountVm.AccountId = accountId;
            await editAccountVm.InitializeCommand.ExecuteAsync();

            // Assert
            editAccountVm.AmountString.ShouldEqual("99.00");
        }


        [Theory]
        [InlineData("de-CH", "12.20", 12.20)]
        [InlineData("de-DE", "12,20", 12.20)]
        [InlineData("en-US", "12.20", 12.20)]
        [InlineData("ru-RU", "12,20", 12.20)]
        [InlineData("de-CH", "-12.20", -12.20)]
        [InlineData("de-DE", "-12,20", -12.20)]
        [InlineData("en-US", "-12.20", -12.20)]
        [InlineData("ru-RU", "-12,20", -12.20)]
        public async Task AmountCorrectlyFormattedOnSave(string cultureString, string amountString, decimal expectedAmount)
        {
            // Arrange
            CultureInfo ci = new CultureInfo(cultureString);
            Thread.CurrentThread.CurrentCulture = ci;
            Thread.CurrentThread.CurrentUICulture = ci;
            ApplicationLanguages.PrimaryLanguageOverride = cultureString;

            var editAccountVm = new EditAccountViewModel(mediatorMock.Object,
                                                         mapperMock.Object,
                                                         dialogServiceMock.Object,
                                                         navigationServiceMock.Object);
            editAccountVm.SelectedAccount.Name = "Foo";

            // Act
            editAccountVm.AmountString = amountString;
            await editAccountVm.SaveCommand.ExecuteAsync();

            // Assert
            editAccountVm.SelectedAccount.CurrentBalance.ShouldEqual(expectedAmount);
        }
    }
}
