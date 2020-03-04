using AutoMapper;
using MediatR;
using MoneyFox.Application.Common.Interfaces;
using MoneyFox.Application.Resources;
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
    public class AddAccountViewModelTests
    {
        [Fact]
        public void Ctor_CategoryCreated()
        {
            // Arrange
            // Act
            var mediator = new Mock<IMediator>();

            var addAccountVm = new AddAccountViewModel(mediator.Object, null, null, null);

            // Assert
            addAccountVm.SelectedAccount.ShouldNotBeNull();
        }

        [Fact]
        public void Ctor_Title_Set()
        {
            // Arrange
            // Act
            var mediator = new Mock<IMediator>();

            var addAccountVm = new AddAccountViewModel(mediator.Object, null, null, null);

            // Assert
            addAccountVm.Title.ShouldEqual(Strings.AddAccountTitle);
        }

        [Fact]
        public async Task AmountStringSetOnInit()
        {
            // Arrange
            CultureInfo ci = new CultureInfo("en-GB");
            Thread.CurrentThread.CurrentCulture = ci;
            Thread.CurrentThread.CurrentUICulture = ci;

            var mediator = new Mock<IMediator>();
            var addAccountVm = new AddAccountViewModel(mediator.Object, null, null, null);

            // Act
            await addAccountVm.InitializeCommand.ExecuteAsync();

            // Assert
            addAccountVm.AmountString.ShouldEqual("0.00");
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

            var mediatorMock = new Mock<IMediator>();
            var mapperMock = new Mock<IMapper>();
            var navigationServiceMock = new Mock<INavigationService>();
            navigationServiceMock.Setup(x => x.GoBack()).Returns(true);

            var dialogServiceMock = new Mock<IDialogService>();

            var addAccountVm = new AddAccountViewModel(mediatorMock.Object,
                                                       mapperMock.Object,
                                                       dialogServiceMock.Object,
                                                       navigationServiceMock.Object);
            addAccountVm.SelectedAccount.Name = "Foo";

            // Act
            addAccountVm.AmountString = amountString;
            await addAccountVm.SaveCommand.ExecuteAsync();

            // Assert
            addAccountVm.SelectedAccount.CurrentBalance.ShouldEqual(expectedAmount);
        }
    }
}
